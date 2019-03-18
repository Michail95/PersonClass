using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D1
{
    public partial class Form_Add : Form
    {
        public bool flag_clear;
        public string col_type;
        Random gen = new Random();
        public Form_Add()
        {
            InitializeComponent();
            comboBox_pol.SelectedIndex = 0;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            Form_main fmain = this.Owner as Form_main;
            if (fmain != null)
            {
                switch (col_type)
                {
                    case "list":
                        if (flag_clear == true)
                            fmain.lmen.Clear();
                        fmain.lmen.Add(new Man(textBox_name.Text, comboBox_pol.Text, int.Parse(textBox_tail.Text), int.Parse(textBox_mas.Text), dateTimePicker_birth.Text));
                        fmain.Reset_DGV();
                        break;
                    case "queue":
                        if (flag_clear == true)
                            fmain.qmen.Clear();
                        fmain.qmen.Enqueue(new Man(textBox_name.Text, comboBox_pol.Text, int.Parse(textBox_tail.Text), int.Parse(textBox_mas.Text), dateTimePicker_birth.Text));
                        fmain.Reset_DGV();
                        break;
                    case "dict":
                        if (flag_clear == true)
                            fmain.dmen.Clear();
                        fmain.dmen.Add(gen.Next(), new Man(textBox_name.Text, comboBox_pol.Text, int.Parse(textBox_tail.Text), int.Parse(textBox_mas.Text), dateTimePicker_birth.Text));
                        fmain.Reset_DGV();
                        break;
                }
            }
        }

        private void is_digit(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void is_letter(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
