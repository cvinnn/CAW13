using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CAW13
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string strconn = "server=localhost;uid=root;pwd=Minato2004-05-05;database=premier_league";
        string query;

        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        MySqlDataReader reader;

        DataTable dt;

        public void prit(string teamid, string manageridnganggur)
        {
            dt = new DataTable();
            query = $"SELECT manager_id FROM manager WHERE manager_name = '{manageridnganggur}'";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            string managerid = dt.Rows[0]["manager_id"].ToString();

            query = $"UPDATE manager SET working = 0 WHERE (manager_id) = (SELECT manager_id from team m WHERE m.team_id = '{teamid}');";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            reader = cmd.ExecuteReader();
            conn.Close();

            query = $"UPDATE team SET manager_id = '{managerid}' WHERE team_id = '{teamid}';";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            reader = cmd.ExecuteReader();
            conn.Close();

            query = $"UPDATE manager SET working = 1 WHERE manager_id = '{managerid}';";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            reader = cmd.ExecuteReader();
            conn.Close();
        }

        public void cbLoader()
        {
            dt = new DataTable();

            query = "SELECT team_id as `id`, team_name as `nat` FROM premier_league.team;";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            cbTeamname.DataSource = dt;
            cbTeamname.ValueMember = "id";
            cbTeamname.DisplayMember = "nat";

            cbTeamname.SelectedItem = "";
            cbTeamname.SelectedValue = "";
        }

        public void dgvLoader()
        {
            dt = new DataTable();

            query = $"select a.manager_name, m.team_name, birthdate, l.nation from premier_league.manager a join premier_league.team m on m.manager_id = a.manager_id join premier_league.nationality l on a.nationality_id = l.nationality_id where team_id = '{cbTeamname.SelectedValue}'";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        public void nganggurLoader()
        {
            dt = new DataTable();

            query = $"select a.manager_name, nation, birthdate from premier_league.manager a join premier_league.nationality n on a.nationality_id = n.nationality_id where working = 0";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            dataGridView2.DataSource = dt;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            cbLoader();
            nganggurLoader();
            this.Dock = DockStyle.Fill;
            this.TopLevel = false;
        }

        private void cbTeamname_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvLoader();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0 || dataGridView2.SelectedCells.Count > 0 
                && cbTeamname.SelectedValue.ToString() != "")
            {
                int rowIndex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[rowIndex];
                string selectedValue = selectedRow.Cells[0].Value.ToString();

                prit(cbTeamname.SelectedValue.ToString(), selectedValue.ToString());

                dgvLoader();
                nganggurLoader();
            }
            else
            {
                MessageBox.Show("Select the manager");
            }
        }
    }
}
