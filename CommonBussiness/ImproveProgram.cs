using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IBussiness
{
    public class ImproveProgram
    {
        public void getmap()
        {
            PersonInfo p1 = new PersonInfo() {  Age=10, Name="张三"};
            PersonInfo p2 = p1.DeepClone();
            p2.Age = 20;
            Console.WriteLine(p1.Age);
        }
    }

    [Serializable]
    public class PersonInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }

        //进行序列化之后的 深拷贝
        public PersonInfo DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formater = new BinaryFormatter();
                formater.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formater.Deserialize(objectStream) as PersonInfo;
            }
        }
    }
}
