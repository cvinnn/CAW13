using System;
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
    public partial class Form4 : Form
    {
        public Form4()
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

        public void dgvLoader(string teamid)
        {
            dt = new DataTable();

            query = $"SELECT player_id, player_name AS Name, nationality_id AS Nationality, playing_pos AS PlayingPosition, team_number AS Number, height, weight, birthdate " +
                $"FROM player WHERE team_id = '{teamid}' and status = 1;";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["player_id"].Visible = false;
        }

        public void deletePlayer(string playerid)
        {
            query = $"DELETE FROM player WHERE player_id = '{playerid}';";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            reader = cmd.ExecuteReader();
            conn.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            cbLoader(); 
            this.Dock = DockStyle.Fill;
            this.TopLevel = false;
        }

        private void cbTeamname_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvLoader(cbTeamname.SelectedValue.ToString());
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 || dataGridView1.SelectedCells.Count > 0
                && cbTeamname.SelectedValue.ToString() != "")
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];
                string selectedValue = selectedRow.Cells["player_id"].Value.ToString();

                deletePlayer(selectedValue);

                dgvLoader(cbTeamname.SelectedValue.ToString());

                MessageBox.Show("Detele Successfully Executed");
            }
            else
            {
                MessageBox.Show("BRUHHHHHHHHH");
            }
        }
    }
}
