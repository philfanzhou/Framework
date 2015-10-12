using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Entity_Framework
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }



        private void button_AddRecord_Click(object sender, EventArgs e)
        {
            var orgCount = PitmanManager.GetAllRecords().Count;

            var personItem = new Person()
            {
                FirstName = "Jack",
                LastName = "Yuan"
            };
            PitmanManager.AddOrUpdate(personItem);

            MessageBox.Show(
                string.Format("原始个数[{0}] 当前个数[{1}]", orgCount, PitmanManager.GetAllRecords().Count), "增加", MessageBoxButtons.OK);
        }

        private void button_DelRecord_Click(object sender, EventArgs e)
        {
            var persons = PitmanManager.GetAllRecords();
            var orgCount = persons.Count;
            if (orgCount == 0)
            {
                MessageBox.Show("删除失败，当前表中记录为0，请新增加一条记录", "删除", MessageBoxButtons.OK);
            }
            else
            {
                var id = persons[0].Id;
                PitmanManager.Delete(id);

                MessageBox.Show(
                    string.Format("原始个数[{0}] 当前个数[{1}]", orgCount, PitmanManager.GetAllRecords().Count), "删除", MessageBoxButtons.OK);
            }
        }

        private void button_EditRecord_Click(object sender, EventArgs e)
        {
            var persons = PitmanManager.GetAllRecords();
            if (persons.Count > 0)
            {
                var orgPerson = persons[0];
                var personId = orgPerson.Id;
                var orgStr = string.Format("Id[{0}] First Name[{1}] LastName[{2}]", orgPerson.Id, orgPerson.FirstName, orgPerson.LastName);

                orgPerson.FirstName = Guid.NewGuid().ToString();
                orgPerson.LastName = Guid.NewGuid().ToString();
                PitmanManager.AddOrUpdate(orgPerson);

                var newPerson = PitmanManager.GetRecordById(personId);
                var curStr = string.Format("Id[{0}] First Name[{1}] LastName[{2}]", newPerson.Id, newPerson.FirstName, newPerson.LastName);

                MessageBox.Show(string.Format("原始记录[{0}]\r\n当前记录[{1}]", orgStr, curStr), "编辑记录", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("记录为0", "编辑记录", MessageBoxButtons.OK);
            }
        }

        private void button_GetAllRecords_Click(object sender, EventArgs e)
        {
            var persons = PitmanManager.GetAllRecords();

            if (persons.Count > 0)
            {
                string outputStr = string.Empty;
                foreach (var record in persons)
                {
                    outputStr += string.Format("Id[{0}] First Name[{1}] LastName[{2}]\r\n", record.Id, record.FirstName, record.LastName);
                }
                MessageBox.Show(outputStr, "查询所有记录", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("记录为0", "查询所有记录", MessageBoxButtons.OK);
            }
        }

        private void button_testconnection_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("修改前ConnectionStrings[{0}]", AppConfigManager.GetConnectionString()), "AppConfig中的连接字符串", MessageBoxButtons.OK);

            AppConfigManager.UpdateConnectionString("test");

            MessageBox.Show(string.Format("修改后ConnectionStrings[{0}]", AppConfigManager.GetConnectionString()), "AppConfig中的连接字符串", MessageBoxButtons.OK);

            AppConfigManager.RestoreToDefaultConnectionString();
        }
    }
}
