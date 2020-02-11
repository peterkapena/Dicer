using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dicer.Model.Dicer
{
    public class Repository : IDicer, IDisposable
    {
        public List<Device> Devices = new List<Device>();
        public List<Person> People = new List<Person>();
        public List<Gamed> gamed = new List<Gamed>();

        public List<Device> AddDevice(Device d)
        {
            Devices.Add(d);
            return Devices;
        }

        public List<Gamed> AddGamed(Gamed g)
        {
            gamed.Add(g);
            return gamed;
        }


        public List<Person> AddPerson(Person p)
        {
            People.Add(p);
            return People;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
