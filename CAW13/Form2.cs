using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CAW13
{
    public partial class Form2 : Form
    {
        public Form2()
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

            query = "select nationality_id as `id`, nation as `nat` from premier_league.nationality";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            cbNationality.DataSource = dt;
            cbNationality.ValueMember = "id";
            cbNationality.DisplayMember = "nat";

            cbNationality.SelectedItem = "";
            cbNationality.SelectedValue = "";

            dt = new DataTable();

            query = "SELECT playing_pos as `id` FROM premier_league.player group by 1";
            conn = new MySqlConnection(strconn);
            conn.Open();
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);

            adapter.Fill(dt);

            cbPosition.DataSource = dt;
            cbPosition.ValueMember = "id";
            cbPosition.DisplayMember = "id";

            cbPosition.SelectedItem = "";
            cbPosition.SelectedValue = "";

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

        private void Form2_Load(object sender, EventArgs e)
        {
            cbLoader();
            this.Dock = DockStyle.Fill;
            this.TopLevel = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtID.Text != null && txtNumber.Text != null && txtName.Text != null && cbNationality.Text != "" && cbPosition.Text != "" && txtHeight.Text != "" && txtWeight.Text != "" && cbTeamname.Text != "")
            {
                MessageBox.Show(cbTeamname.SelectedValue.ToString());
                query = $"insert into premier_league.player (player_id, team_number, player_name, nationality_id, playing_pos, height, weight, birthdate, team_id, status, `delete`) " +
                    $"values ('{txtID.Text}', {txtNumber.Text}, '{txtName.Text}', '{cbNationality.SelectedValue.ToString()}', '{cbPosition.SelectedValue.ToString()}', {txtHeight.Text}, {txtWeight.Text}, '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}', '{cbTeamname.SelectedValue.ToString()}', 1, 0);";
                MessageBox.Show(query);
                
                conn = new MySqlConnection(strconn);
                conn.Open();
                cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();
                conn.Close();

                MessageBox.Show("Player Successfully Added");

                txtID.Text = "";
                txtNumber.Text = "";
                txtName.Text = "";
                txtHeight.Text = "";
                txtWeight.Text = "";

                cbNationality.SelectedItem = "";
                cbNationality.SelectedValue = "";

                cbPosition.SelectedItem = "";
                cbPosition.SelectedValue = "";

                cbTeamname.SelectedItem = "";
                cbTeamname.SelectedValue = "";

                dateTimePicker1.Value = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Isi yang lengkap");
            }
        }
    }
}
