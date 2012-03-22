using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;

namespace ComboEditor
{   
    public partial class Form1 : Form
    {
        List<Combo> list = new List<Combo>();

        public Form1()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           Combo obj1 = new Combo()
            {
                Name = "Fire",
                How = "F",
                Description = "Some fire"
            };

            Combo obj2 = new Combo()
            {
                Name = "Ice",
                How = "C",
                Description = "Some ice"
            };

            
            list.Add(obj1);
            list.Add(obj2);

            dataGridView1.DataSource = list;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            Combo objNew = new Combo()
            {
                Name = "Name",
                How = "How",
                Description = "description"
            };
            list.Add(objNew);           
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }

        static List<Combo> DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Combo>));
            TextReader textReader = new StreamReader(@"combos.xml");
            List<Combo> combo;
            combo = (List<Combo>)deserializer.Deserialize(textReader);
            textReader.Close();

            return combo;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            list = DeserializeFromXML();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SerializeToXML(list);
        }

        static public void SerializeToXML(List<Combo> combo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Combo>));
            TextWriter textWriter = new StreamWriter("combos.xml");
            serializer.Serialize(textWriter, combo);
            textWriter.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            list.Clear();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }
    }

    public class Combo
    {
        public string Name { get; set; }
        public string How { get; set; }
        public string Description { get; set; }
    }   
}


