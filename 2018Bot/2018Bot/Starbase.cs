using Flattiverse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2018Bot
{
    public class Starbase
    {
        public Connector Connector;
        public Team Team;
        public delegate void Ready();
        public event Ready ReadyEvent;
        public Starship SelectedShip;
        public List<Starship> StarShipHolder = new List<Starship>();
        public List<Map> BaseMapHolder = new List<Map>();
        public List<string> Messages = new List<string>();

        public object SyncMessages = new object();
        private object syncBaseMap = new object();

        private UniverseGroup universeGroup;
        private FlattiverseMessage message;

        public void Connect(string email, string password)
        {
            if (File.Exists("benchmark"))
            {
                Connector.LoadBenchmark(File.ReadAllBytes("benchmark"));
            }
            else
            {
                Connector.DoBenchmark();
                File.WriteAllBytes("benchmark", Connector.SaveBenchmark());
            }

            Connector = new Connector(email, password);
            universeGroup = Connector.UniverseGroups["Mission I"];
            Team = universeGroup.Teams["None"];
            //universeGroup.Join("Scharle", Team, null, "l%har256");
            universeGroup.Join("Violet" +
                "", Team);
            Thread thread = new Thread(baseRotation);
            thread.Name = "StarBase";
            thread.Start();
        }

        public void CreateNewShip()
        {
            Starship ship = new Starship(this, universeGroup);
            StarShipHolder.Add(ship);
            SelectedShip = ship;
        }

        public void SetNewDestination(Vector klick)
        {
            SelectedShip.NewDestination = new List<Vector>() { klick };
        }

        public Map MergeAll(Map map)
        {
            if (!map.IsMergable)
            {
                return map;
            }

            lock (syncBaseMap)
            {
                foreach (Map sMap in BaseMapHolder)
                    if (sMap.Merge(map))
                        return sMap;

                BaseMapHolder.Add(map);
            }

            return map;
        }

        private void baseRotation()
        {
            UniverseGroupFlowControl flowControl = universeGroup.GetNewFlowControl();

            while (true)
            {
                flowControl.Commit();
                flowControl.Wait();
                
                while (Connector.NextMessage(out message))
                    lock (SyncMessages)
                        Messages.AddRange(message.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList());

                ReadyEvent();
            }
        }
    }
}
