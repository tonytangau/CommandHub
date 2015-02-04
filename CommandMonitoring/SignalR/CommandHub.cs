using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommandMonitoring.Models;
using CommandMonitoring.Services;
using Microsoft.AspNet.SignalR;

namespace CommandMonitoring.SignalR
{
    public class CommandHub : Hub
    {
        // Is set via the constructor on each creation
        private Broadcaster _broadcaster;

        public CommandHub()
            : this(Broadcaster.Instance)
        {
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

        public IEnumerable<DrillHole> Read()
        {
            return _broadcaster.GetDrillHoles();
        }
    }

    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());

        // We're going to broadcast to all clients a maximum of 2 times per second
        private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(1500);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<CommandHub>();

            // Start the broadcast loop
            //_broadcastLoop = new Timer(
            //    BroadcastUpdate,
            //    null,
            //    BroadcastInterval,
            //    BroadcastInterval);
        }

        public void BroadcastUpdate(DrillHole newDrillHole)// object state, then get most recent items?
        {
            if (newDrillHole != null)
            {
                _hubContext.Clients.All.broadcastMessage("New drill hole data: " + newDrillHole);
            }
        }

        public IEnumerable<DrillHole> GetDrillHoles()
        {
            var repo = new DrillHoleRepository();

            // We only show data from last 2 mins
            var lowerBoundTime = DateTime.UtcNow.AddMinutes(-2);

            return repo.GetHolesForProject(1).OrderByDescending(h => h.DrillHoleId).ToList().Take(20).Where(h => h.TimeStamp.ToUniversalTime() > lowerBoundTime);
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