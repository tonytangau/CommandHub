using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandMonitoring.Models;
using CommandMonitoring.Services;
using Microsoft.AspNet.SignalR;

namespace CommandMonitoring.SignalR
{
    public class CommandHub : Hub
    {
        // Is set via the constructor on each creation
        private Broadcaster _broadcaster;
        private static int _lowerBoundInSeconds;
        private static int _numberOfReadings;

        public CommandHub(): this(Broadcaster.Instance)
        {
            // Default value if App setting is missing
            _lowerBoundInSeconds = 120;
            _numberOfReadings = 20;

            int.TryParse(ConfigurationManager.AppSettings["LowerBoundInSeconds"], out _lowerBoundInSeconds);
            int.TryParse(ConfigurationManager.AppSettings["NumberOfReadings"], out _numberOfReadings);
        }

        public CommandHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateDisplay(DrillHole newDrillHole)
        {
            // Update the shape model within our broadcaster
            _broadcaster.BroadcastUpdate(newDrillHole);
        }

        public async Task<IEnumerable<DrillHole>> Read()
        {
            return await _broadcaster.GetDrillHoles();
        }

        public class Broadcaster
        {
            private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
 
            private readonly IHubContext _hubContext;
            private HubContext _context;

            public Broadcaster()
            {
                // Save our hub context so we can easily use it to send to its connected clients
                _hubContext = GlobalHost.ConnectionManager.GetHubContext<CommandHub>();
                _context = new HubContext();
            }

            public void BroadcastUpdate(DrillHole newDrillHole)
            {
                if (newDrillHole != null)
                {
                    _hubContext.Clients.All.broadcastMessage();
                }
            }

            public async Task<IEnumerable<DrillHole>> GetDrillHoles()
            {
                var result = await _context.DrillHoles.Where(h => h.ProjectId == 1).OrderByDescending(h => h.DrillHoleId).Take(_numberOfReadings).ToListAsync();

                var lowerBoundTime = DateTime.UtcNow.AddSeconds(-_lowerBoundInSeconds);

                return result.Where(h => h.TimeStamp.ToUniversalTime() > lowerBoundTime);
            }

            public static Broadcaster Instance
            {
                get
                {
                    return _instance.Value;
                }
            }
        }
    }
}