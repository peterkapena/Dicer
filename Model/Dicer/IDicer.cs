using System.Collections.Generic;

namespace Dicer.Model.Dicer
{
    public interface IDicer
    {
        public List<Person> AddPerson(Person p);
        public List<Device> AddDevice(Device d);
        public List<Gamed> AddGamed(Gamed g);
    }
}