using System;
using System.Collections.Generic;
using System.Linq;
using CommandMonitoring.Models;
using CommandMonitoring.Services;
using Microsoft.AspNet.SignalR;

namespace CommandMonitoring.SignalR
{
    public class CommandHub : Hub
    {
        public void UpdateDisplay(DrillHole newDrillHole)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CommandHub>();
            hubContext.Clients.All.broadcastMessage("New Drill hole data arrived: " + DateTime.Now + " " + newDrillHole);
            hubContext.Clients.All.updateChart();
        }

        public IEnumerable<DrillHole> Read()
        {
            var repo = new DrillHoleRepository();

            return repo.GetHolesForProject(1).ToList().Take(10);
        }
    }

    //public class CommandHub : Hub
    //{
    //    // Is set via the constructor on each creation
    //    private Broadcaster _broadcaster;

    //    public CommandHub()
    //        : this(Broadcaster.Instance)
    //    {
    //    }

    //    public CommandHub(Broadcaster broadcaster)
    //    {
    //        _broadcaster = broadcaster;
    //    }

    //    public void UpdateDisplay(DrillHole newDrillHole)
    //    {
    //        // Update the shape model within our broadcaster
    //        _broadcaster.BroadcastUpdate(newDrillHole);
    //    }

    //    public IEnumerable<DrillHole> Read()
    //    {
    //        var repo = new DrillHoleRepository();

    //        return repo.GetHolesForProject(1).ToList().Take(10);
    //    }
    //}

    //public class Broadcaster
    //{
    //    private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());

    //    // We're going to broadcast to all clients a maximum of 2 times per second
    //    private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(1500);
    //    private readonly IHubContext _hubContext;
    //    private Timer _broadcastLoop;

    //    public Broadcaster()
    //    {
    //        // Save our hub context so we can easily use it to send to its connected clients
    //        _hubContext = GlobalHost.ConnectionManager.GetHubContext<CommandHub>();

    //        // Start the broadcast loop
    //        _broadcastLoop = new Timer(
    //            BroadcastUpdate,
    //            null,
    //            BroadcastInterval,
    //            BroadcastInterval);
    //    }

    //    public void BroadcastUpdate(object newDrillHole)// object state, then get most recent items?
    //    {
    //        if (newDrillHole != null)
    //        {
    //            _hubContext.Clients.All.broadcastMessage("New Drill hole data arrived: " + DateTime.Now + " " + newDrillHole);
    //        }
    //        else
    //        {
    //            _hubContext.Clients.All.broadcastMessage("No new drill hole\t" + DateTime.Now);
    //        }
    //    }

    //    public static Broadcaster Instance
    //    {
    //        get
    //        {
    //            return _instance.Value;
    //        }
    //    }
    //}
}