using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Circuits
{
    public class InputSource : Gate
    {
        //fields
        private bool _voltage;

        //whether the gate is on or off (0 or 1)
        private int VoltageValue = 0;


        /// <summary>
        /// Initialises the input source
        /// </summary>
        /// <param name="x">mouse x position</param>
        /// <param name="y">mouse y position</param>
        public InputSource(int x, int y) : base(x, y)
        {
            pins.Add(new Pin(this, false, 20));
            MoveTo(x, y);
            _voltage = false;

        }


        /// <summary>
        /// Gets and sets the voltage
        /// </summary>
        public bool Voltage
        {
            get { return _voltage; }
            set { _voltage = value; }
        }
        /// <summary>
        /// Changes the VoltageValue and voltage of the gate
        /// </summary>
        public void ChangeSelected()
        {
            //if gate is selected
            if (selected)
            {
                //if VoltageValue is 0
                if (VoltageValue == 0)
                {
                    //set VoltageValue to 1 and change voltage to true
                    VoltageValue = 1;
                    _voltage = true;
                }
                else
                {
                    //set VoltageValue to 0 and change voltage to false
                    VoltageValue = 0;
                    _voltage = false;
                }
            }
        }


        /// <summary>
        /// Draws the gate in the normal colour or in the selected colour.
        /// </summary>
        /// <param name="paper">The drawing object</param>
        public override void Draw(Graphics paper)
        {
            //Draw each of the pins
            foreach (Pin p in pins)
                p.Draw(paper);

            // Create font and brush.
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
                //Check if the gate has been selected
                if (selected)
                {
                    //draw input source in red with the voltage value
                    paper.DrawImage(Properties.Resources.InputIcon, Left, Top);
                    paper.DrawString(VoltageValue.ToString(), drawFont, drawBrush, Left - 15, Top);
                    paper.FillRectangle(Brushes.Red, Left + 3, Top + 7, GAP - 3, GAP - 3);
                }
                else
                {
                    //draw input source in gray with the voltage value
                    paper.DrawImage(Properties.Resources.InputIcon, Left, Top);
                    paper.DrawString(VoltageValue.ToString(), drawFont, drawBrush, Left - 15, Top);
                    paper.FillRectangle(Brushes.Gray, Left + 3, Top + 7, GAP - 3, GAP - 3);

                }
        }


        /// <summary>
        /// Moves the gate to the position specified.
        /// </summary>
        /// <param name="x">The x position to move the gate to</param>
        /// <param name="y">The y position to move the gate to</param>
        public override void MoveTo(int x, int y)
        {
            //Debugging message
            Console.WriteLine("pins = " + pins.Count);
            //Set the position of the gate to the values passed in
            left = x;
            top = y;
            // must move the pins too
            pins[0].X = x + 35;
            pins[0].Y = y + 10;
        }



        /// <summary>
        /// Checks the voltage of the gate
        /// </summary>
        /// <returns>The voltage(whether it's on or off)</returns>
        public override bool Evaluate()
        {
            return _voltage;   
        }

        /// <summary>
        /// Clones the gate
        /// </summary>
        /// <param name="g"></param>
        /// <returns>new input source</returns>
        public override Gate Clone(Gate g)
        {
            g = new InputSource(0, 0);
            return g;
        }
    }
}
