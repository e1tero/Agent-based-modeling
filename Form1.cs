using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab18
{           
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        double lamda = 4.1,lamda1 = 3.2, currentTime = 0;
        List<agent> Agents = new List<agent>();
        int NumberC = 4, NumberBusy = 0;
        public double random(double lambda)
        {
            double t = 3*lambda * Math.Pow(Math.E, -1*lambda * rnd.NextDouble());
            return t; 
        }
        public class agent
        {
            public double time;
            public int state;
            public agent(double time)
            {
                this.time = time;
                this.state = 0;
            }  
            public int curState()
            {
                return state;
            }
            public void go(double newTime)
            {
                this.state = 1;
                this.time = newTime;
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NumberC = 4;
            NumberBusy = 0;
            Agents.Clear();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.ChartAreas[0].AxisX.Maximum = 30;
            //chart1.ChartAreas[0].AxisX.Minimum = 0;
            for (int i = 0; i < Convert.ToInt32(number.Text); i++)
            {
                double time = random(lamda);
                agent newagent = new agent(time);
                Agents.Add(newagent);
            }
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Agents.Sort(delegate (agent x, agent y)
            {
                if (x.time <= y.time) return -1;
                else return 1;
            });

            if (Agents.Count > 0)
            {
                for (int agent = 0; agent < Agents.Count; agent++)
                {
                    if (Agents[agent].curState() == 1)
                    {
                        chart1.Series[1].Points.AddXY(Agents[agent].time, 0);
                        currentTime = Agents[agent].time;
                        Agents.Remove(Agents[agent]);
                        NumberBusy--;
                        break;
                    }
                    else if (NumberBusy < NumberC)
                    {
                        NumberBusy++;
                        if (Agents[agent].time < currentTime)
                        {
                            chart1.Series[0].Points.AddXY(currentTime, 0);
                            Agents[agent].go(random(lamda1) + currentTime);
                            break;
                        }
                        chart1.Series[0].Points.AddXY(Agents[agent].time, 0);
                        Agents[agent].go(random(lamda1) + Agents[agent].time);
                        break;
                    }
                }
            }

        }
    }
}
