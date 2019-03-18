using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace D1
{
    public partial class Form_main : Form
    {
        public List<Man> lmen = new List<Man>();
        public Queue<Man> qmen = new Queue<Man>();
        public Dictionary<int, Man> dmen = new Dictionary<int, Man>();

        string[] names;
        int cnt_names;
        string[] pols = { "М", "Ж" };
        DateTime start = new DateTime(1960, 1, 1);
        Random gen = new Random();

        public Form_main()
        {
            InitializeComponent();
            comboBox_pole.SelectedIndex = 0;
            comboBox_pol.SelectedIndex = 0;
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            try
            {
                names = File.ReadAllLines("..//..//Resources//men_for_random.txt");
                cnt_names = names.Length;
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        private void rbtn_check_changed(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            switch (btn.Name)
            {
                case "radioButton_list":
                    Reset_DGV();
                    Load_DGV();
                    break;
                case "radioButton_q":
                    Reset_DGV();
                    Load_DGV();
                    break;
                case "radioButton_dict":
                    Reset_DGV();
                    Load_DGV();
                    break;
            }
        }

        void Load_DGV()
        {
            try
            {
                dataGridView_main.Columns[0].HeaderText = "Имя";
                dataGridView_main.Columns[1].HeaderText = "Пол";
                dataGridView_main.Columns[2].HeaderText = "Рост";
                dataGridView_main.Columns[3].HeaderText = "Вес";
                dataGridView_main.Columns[4].HeaderText = "Дата рождения";
                dataGridView_main.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        public void Reset_DGV()
        {
            dataGridView_main.DataSource = null;
            BindingSource bind = new BindingSource();
            bind.SuspendBinding();

            if (radioButton_list.Checked)
            {
                bind.DataSource = lmen;
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
            }
            if (radioButton_q.Checked)
            {
                bind.DataSource = qmen.ToList();
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
            }
            if (radioButton_dict.Checked)
            {
                bind.DataSource = dmen.Values.ToList();
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
            }
            Load_DGV();
        }

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);

            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private Man RandMan()
        {
            return new Man(names[gen.Next(cnt_names)], pols[gen.Next(2)],
                gen.Next(150, 195), gen.Next(50, 110), RandomDay().ToShortDateString().ToString());
        }

        private void button_rand_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag_clear = false;
                if (MessageBox.Show("Очистить коллекцию?", "Запись", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    flag_clear = true;
                }
                if (radioButton_list.Checked)
                {
                    if (flag_clear == true)
                        lmen.Clear();
                    for (int i = 0; i < gen.Next(1, cnt_names); i++)
                    {
                        lmen.Add(RandMan());
                    }
                }
                if (radioButton_q.Checked)
                {
                    if (flag_clear == true)
                        qmen.Clear();
                    for (int i = 0; i < gen.Next(1, cnt_names); i++)
                    {
                        qmen.Enqueue(RandMan());
                    }
                }
                if (radioButton_dict.Checked)
                {
                    if (flag_clear == true)
                        dmen.Clear();
                    for (int i = 0; i < gen.Next(1, cnt_names); i++)
                    {
                        dmen.Add(gen.Next(), RandMan());
                    }
                }
                Reset_DGV();
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        private void button_file_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag_clear = false;
                var f = new StreamReader(new FileStream("..//..//Resources//men.txt", FileMode.Open), Encoding.GetEncoding(1251));
                if (MessageBox.Show("Очистить коллекцию?", "Запись", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    flag_clear = true;
                }
                while (true)
                {
                    string text = f.ReadLine();
                    if (text == null)
                        break;
                    string[] data = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (radioButton_list.Checked)
                    {
                        if (flag_clear == true)
                            lmen.Clear();
                        lmen.Add(new Man(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]), data[4]));
                    }
                    if (radioButton_q.Checked)
                    {
                        if (flag_clear == true)
                            qmen.Clear();
                        qmen.Enqueue(new Man(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]), data[4]));
                    }
                    if (radioButton_dict.Checked)
                    {
                        if (flag_clear == true)
                            dmen.Clear();
                        dmen.Add(gen.Next(), new Man(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]), data[4]));
                    }
                }
                f.Close();
                Reset_DGV();
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Add fadd = new Form_Add();
                if (MessageBox.Show("Очистить коллекцию?", "Запись", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    fadd.flag_clear = true;
                }
                if (radioButton_list.Checked) { fadd.col_type = "list"; }

                if (radioButton_q.Checked) { fadd.col_type = "queue"; }

                if (radioButton_dict.Checked) { fadd.col_type = "dict"; }
                fadd.FormClosed += new FormClosedEventHandler(Form_add_closed);
                fadd.Owner = this;
                fadd.Show();
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        void Form_add_closed(object sender, FormClosedEventArgs e) { Reset_DGV(); }

        private void button_clear_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton_list.Checked) { lmen.Clear(); }
                if (radioButton_q.Checked) { qmen.Clear(); }
                if (radioButton_dict.Checked) { dmen.Clear(); }
                Reset_DGV();
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        private void is_letter(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        void Write_Logs(string log)
        {
            var f = new StreamWriter(new FileStream("..//..//Resources//logs.txt", FileMode.Append));
            f.WriteLine(log);
            f.Close();
        }

        private void button_find_del_Click(object sender, EventArgs e)
        {
            var tmp_lmen = new List<Man>();
            var tmp_qmen = new Queue<Man>();
            var tmp_dmen = new Dictionary<int, Man>();

            if (radioButton_list.Checked)   ////////////////list
            {
                switch (trackBar_type_find.Value)
                {
                    case 1:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in lmen)
                            {
                                if (man.tail >= numericUpDown_min.Value && man.tail <= numericUpDown_max.Value)
                                    tmp_lmen.Add(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in lmen)
                            {
                                if (!(man.tail >= numericUpDown_min.Value || man.tail <= numericUpDown_max.Value))
                                    tmp_lmen.Add(man);
                            }
                            lmen.Clear();
                            lmen = tmp_lmen;
                        }
                        break;
                    case 2:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in lmen)
                            {
                                if (man.mas >= numericUpDown_min.Value && man.mas <= numericUpDown_max.Value)
                                    tmp_lmen.Add(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in lmen)
                            {
                                if (!(man.mas >= numericUpDown_min.Value || man.mas <= numericUpDown_max.Value))
                                    tmp_lmen.Add(man);
                            }
                            lmen.Clear();
                            lmen = tmp_lmen;
                        }
                        break;
                    case 3:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in lmen)
                            {
                                DateTime date = DateTime.Parse(man.birth);
                                if (date >= dateTimePicker_min.Value && date <= dateTimePicker_max.Value)
                                    tmp_lmen.Add(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in lmen)
                            {
                                DateTime date = DateTime.Parse(man.birth);
                                if (!(date >= dateTimePicker_min.Value || date <= dateTimePicker_max.Value))
                                    tmp_lmen.Add(man);
                            }
                            lmen.Clear();
                            lmen = tmp_lmen;
                        }
                        break;
                    case 4:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in lmen)
                            {
                                if (string.Compare(man.pol, comboBox_pol.Text) == 0)
                                    tmp_lmen.Add(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in lmen)
                            {
                                if (!(string.Compare(man.pol, comboBox_pol.Text) == 0))
                                    tmp_lmen.Add(man);
                            }
                            lmen.Clear();
                            lmen = tmp_lmen;
                        }
                        break;
                    case 5:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in lmen)
                            {
                                if (string.Compare(man.name, textBox_name.Text) == 0)
                                    tmp_lmen.Add(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in lmen)
                            {
                                if (!(string.Compare(man.name, textBox_name.Text) == 0))
                                    tmp_lmen.Add(man);
                            }
                            lmen.Clear();
                            lmen = tmp_lmen;
                        }
                        break;
                }
                dataGridView_main.DataSource = null;
                BindingSource bind = new BindingSource();
                bind.SuspendBinding();
                bind.DataSource = tmp_lmen;
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
                Load_DGV();
            }
            if (radioButton_q.Checked)  ////////////////queue
            {
                switch (trackBar_type_find.Value)
                {
                    case 1:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in qmen)
                            {
                                if (man.tail >= numericUpDown_min.Value && man.tail <= numericUpDown_max.Value)
                                    tmp_qmen.Enqueue(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in qmen)
                            {
                                if (!(man.tail >= numericUpDown_min.Value || man.tail <= numericUpDown_max.Value))
                                    tmp_qmen.Enqueue(man);
                            }
                            qmen.Clear();
                            qmen = tmp_qmen;
                        }
                        break;
                    case 2:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in qmen)
                            {
                                if (man.mas >= numericUpDown_min.Value && man.mas <= numericUpDown_max.Value)
                                    tmp_qmen.Enqueue(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in qmen)
                            {
                                if (!(man.mas >= numericUpDown_min.Value || man.mas <= numericUpDown_max.Value))
                                    tmp_qmen.Enqueue(man);
                            }
                            qmen.Clear();
                            qmen = tmp_qmen;
                        }
                        break;
                    case 3:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in qmen)
                            {
                                DateTime date = DateTime.Parse(man.birth);
                                if (date >= dateTimePicker_min.Value && date <= dateTimePicker_max.Value)
                                    tmp_qmen.Enqueue(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in qmen)
                            {
                                DateTime date = DateTime.Parse(man.birth);
                                if (!(date >= dateTimePicker_min.Value || date <= dateTimePicker_max.Value))
                                    tmp_qmen.Enqueue(man);
                            }
                            qmen.Clear();
                            qmen = tmp_qmen;
                        }
                        break;
                    case 4:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in qmen)
                            {
                                if (string.Compare(man.pol, comboBox_pol.Text) == 0)
                                    tmp_qmen.Enqueue(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in qmen)
                            {
                                if (!(string.Compare(man.pol, comboBox_pol.Text) == 0))
                                    tmp_qmen.Enqueue(man);
                            }
                            qmen.Clear();
                            qmen = tmp_qmen;
                        }
                        break;
                    case 5:
                        if (radioButton_find.Checked)
                        {
                            foreach (Man man in qmen)
                            {
                                if (string.Compare(man.name, textBox_name.Text) == 0)
                                    tmp_qmen.Enqueue(man);
                            }
                        }
                        else
                        {
                            foreach (Man man in qmen)
                            {
                                if (!(string.Compare(man.name, textBox_name.Text) == 0))
                                    tmp_qmen.Enqueue(man);
                            }
                            qmen.Clear();
                            qmen = tmp_qmen;
                        }
                        break;
                }
                dataGridView_main.DataSource = null;
                BindingSource bind = new BindingSource();
                bind.SuspendBinding();
                bind.DataSource = tmp_qmen.ToList();
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
                Load_DGV();
            }
            if (radioButton_dict.Checked)  ////////////////dictionary
            {
                switch (trackBar_type_find.Value)
                {
                    case 1:
                        if (radioButton_find.Checked)
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (man.Value.tail >= numericUpDown_min.Value && man.Value.tail <= numericUpDown_max.Value)
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (!(man.Value.tail >= numericUpDown_min.Value || man.Value.tail <= numericUpDown_max.Value))
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                            dmen.Clear();
                            dmen = tmp_dmen;
                        }
                        break;
                    case 2:
                        if (radioButton_find.Checked)
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (man.Value.mas >= numericUpDown_min.Value && man.Value.mas <= numericUpDown_max.Value)
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (!(man.Value.mas >= numericUpDown_min.Value || man.Value.mas <= numericUpDown_max.Value))
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                            dmen.Clear();
                            dmen = tmp_dmen;
                        }
                        break;
                    case 3:
                        if (radioButton_find.Checked)
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                DateTime date = DateTime.Parse(man.Value.birth);
                                if (date >= dateTimePicker_min.Value && date <= dateTimePicker_max.Value)
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                DateTime date = DateTime.Parse(man.Value.birth);
                                if (!(date >= dateTimePicker_min.Value || date <= dateTimePicker_max.Value))
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                            dmen.Clear();
                            dmen = tmp_dmen;
                        }
                        break;
                    case 4:
                        if (radioButton_find.Checked)
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (string.Compare(man.Value.pol, comboBox_pol.Text) == 0)
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (!(string.Compare(man.Value.pol, comboBox_pol.Text) == 0))
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                            dmen.Clear();
                            dmen = tmp_dmen;
                        }
                        break;
                    case 5:
                        if (radioButton_find.Checked)
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (string.Compare(man.Value.name, textBox_name.Text) == 0)
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<int, Man> man in dmen)
                            {
                                if (!(string.Compare(man.Value.name, textBox_name.Text) == 0))
                                    tmp_dmen.Add(man.Key, man.Value);
                            }
                            dmen.Clear();
                            dmen = tmp_dmen;
                        }
                        break;
                }
                dataGridView_main.DataSource = null;
                BindingSource bind = new BindingSource();
                bind.SuspendBinding();
                bind.DataSource = tmp_dmen.Values.ToList();
                bind.ResumeBinding();
                dataGridView_main.DataSource = bind;
                Load_DGV();
            }
        }

        private void button_sort_Click(object sender, EventArgs e)
        {
            if (radioButton_list.Checked)    ///////////list
            {
                switch (comboBox_pole.SelectedIndex)
                {
                    case 0:
                        if (radioButton_up.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return x.name.CompareTo(y.name);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return -x.name.CompareTo(y.name);
                            });
                        }
                        break;
                    case 1:
                        if (radioButton_up.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return x.pol.CompareTo(y.pol);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return -x.pol.CompareTo(y.pol);
                            });
                        }
                        break;
                    case 2:
                        if (radioButton_up.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return x.tail.CompareTo(y.tail);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return -x.tail.CompareTo(y.tail);
                            });
                        }
                        break;
                    case 3:
                        if (radioButton_up.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return x.mas.CompareTo(y.mas);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                return -x.mas.CompareTo(y.mas);
                            });
                        }
                        break;
                    case 4:
                        if (radioButton_up.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                DateTime d1 = DateTime.Parse(x.birth);
                                DateTime d2 = DateTime.Parse(y.birth);
                                return d1.CompareTo(d2);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            lmen.Sort(delegate(Man x, Man y)
                            {
                                DateTime d1 = DateTime.Parse(x.birth);
                                DateTime d2 = DateTime.Parse(y.birth);
                                return -d1.CompareTo(d2);
                            });
                        }
                        break;
                }
            }
            if (radioButton_q.Checked)   /////////////queue
            {
                var tmp_list = qmen.ToList();
                switch (comboBox_pole.SelectedIndex)
                {
                    case 0:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return x.name.CompareTo(y.name);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return -x.name.CompareTo(y.name);
                            });
                        }
                        break;
                    case 1:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return x.pol.CompareTo(y.pol);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return -x.pol.CompareTo(y.pol);
                            });
                        }
                        break;
                    case 2:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return x.tail.CompareTo(y.tail);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return -x.tail.CompareTo(y.tail);
                            });
                        }
                        break;
                    case 3:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return x.mas.CompareTo(y.mas);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                return -x.mas.CompareTo(y.mas);
                            });
                        }
                        break;
                    case 4:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                DateTime d1 = DateTime.Parse(x.birth);
                                DateTime d2 = DateTime.Parse(y.birth);
                                return d1.CompareTo(d2);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(Man x, Man y)
                            {
                                DateTime d1 = DateTime.Parse(x.birth);
                                DateTime d2 = DateTime.Parse(y.birth);
                                return -d1.CompareTo(d2);
                            });
                        }
                        break;
                }
                qmen.Clear();
                qmen = new Queue<Man>(tmp_list);
            }
            if (radioButton_dict.Checked)   /////////////dictionary
            {
                var tmp_list = dmen.ToList();
                switch (comboBox_pole.SelectedIndex)
                {
                    case 0:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return x.Value.name.CompareTo(y.Value.name);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return -x.Value.name.CompareTo(y.Value.name);
                            });
                        }

                        break;
                    case 1:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return x.Value.pol.CompareTo(y.Value.pol);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return -x.Value.pol.CompareTo(y.Value.pol);
                            });
                        }
                        break;
                    case 2:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return x.Value.tail.CompareTo(y.Value.tail);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return -x.Value.tail.CompareTo(y.Value.tail);
                            });
                        }
                        break;
                    case 3:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return x.Value.mas.CompareTo(y.Value.mas);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                return -x.Value.mas.CompareTo(y.Value.mas);
                            });
                        }
                        break;
                    case 4:
                        if (radioButton_up.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                DateTime d1 = DateTime.Parse(x.Value.birth);
                                DateTime d2 = DateTime.Parse(y.Value.birth);
                                return d1.CompareTo(d2);
                            });
                        }
                        if (radioButton_down.Checked)
                        {
                            tmp_list.Sort(delegate(KeyValuePair<int, Man> x, KeyValuePair<int, Man> y)
                            {
                                DateTime d1 = DateTime.Parse(x.Value.birth);
                                DateTime d2 = DateTime.Parse(y.Value.birth);
                                return -d1.CompareTo(d2);
                            });
                        }
                        break;
                }
                dmen.Clear();
                dmen = tmp_list.ToDictionary(x => x.Key, x => x.Value);
            }
            Reset_DGV();
        }

        void rbtn_upr_check_changed(object sender, EventArgs e)
        {
            try
            {
                RadioButton btn = (RadioButton)sender;
                switch (btn.Name)
                {
                    case "radioButton_find":
                        groupBox_main.Text = "Поиск";
                        button_find_del.Text = "Поиск";
                        groupBox_main.Visible = true;
                        groupBox_sort.Visible = false;
                        break;
                    case "radioButton_sort":
                        groupBox_main.Visible = false;
                        groupBox_sort.Visible = true;
                        break;
                    case "radioButton_del":
                        groupBox_main.Text = "Удалить";
                        button_find_del.Text = "Удалить";
                        groupBox_main.Visible = true;
                        groupBox_sort.Visible = false;
                        break;
                }
                Reset_DGV();
            }
            catch (Exception ex) { Write_Logs(ex.Message); }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            Reset_DGV();
        }

        private void trackBar_type_find_Scroll(object sender, EventArgs e)
        {
            switch (trackBar_type_find.Value)
            {
                case 1:
                    label_type.Text = "Рост";
                    numericUpDown_min.Visible = true;
                    numericUpDown_max.Visible = true;
                    label3.Visible = true;
                    dateTimePicker_min.Visible = false;
                    dateTimePicker_max.Visible = false;
                    comboBox_pol.Visible = false;
                    textBox_name.Visible = false;
                    break;
                case 2:
                    label_type.Text = "Вес";
                    numericUpDown_min.Visible = true;
                    numericUpDown_max.Visible = true;
                    label3.Visible = true;
                    dateTimePicker_min.Visible = false;
                    dateTimePicker_max.Visible = false;
                    comboBox_pol.Visible = false;
                    textBox_name.Visible = false;
                    break;
                case 3:
                    label_type.Text = "Возраст";
                    numericUpDown_min.Visible = false;
                    numericUpDown_max.Visible = false;
                    label3.Visible = true;
                    dateTimePicker_min.Visible = true;
                    dateTimePicker_max.Visible = true;
                    comboBox_pol.Visible = false;
                    textBox_name.Visible = false;
                    break;
                case 4:
                    label_type.Text = "Пол";
                    numericUpDown_min.Visible = false;
                    numericUpDown_max.Visible = false;
                    label3.Visible = false;
                    dateTimePicker_min.Visible = false;
                    dateTimePicker_max.Visible = false;
                    comboBox_pol.Visible = true;
                    textBox_name.Visible = false;
                    break;
                case 5:
                    label_type.Text = "Имя";
                    numericUpDown_min.Visible = false;
                    numericUpDown_max.Visible = false;
                    label3.Visible = false;
                    dateTimePicker_min.Visible = false;
                    dateTimePicker_max.Visible = false;
                    comboBox_pol.Visible = false;
                    textBox_name.Visible = true;
                    break;
            }
        }
    }
}