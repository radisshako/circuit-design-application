using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Circuits
{
    public class OutputLamp : Gate
    {
        //fields

        /// <summary>
        /// output of the lamp
        /// </summary>
        private bool outputStatus;


        /// <summary>
        /// Initialises the output lamp
        /// </summary>
        /// <param name="x">mouse x position</param>
        /// <param name="y">mouse y position</param>
        public OutputLamp(int x, int y) : base(x, y)
        {
            pins.Add(new Pin(this, true, 20));
            MoveTo(x, y);
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

                if (outputStatus == true) //if gate is on
                {
                    drawBrush.Color = Color.Yellow; //set brush to yellow
            }
                else if (outputStatus == false) //if gate is off
                {
                    drawBrush.Color = Color.Gray; //set brush to gray
            }
                if (selected) //if gate is selected
            {
                    drawBrush.Color = Color.Red; //set brush to red
                }

            //draw the gate
            paper.FillRectangle(drawBrush, Left, Top, GAP * 2, GAP * 2);




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
            pins[0].X = x - 10;
            pins[0].Y = y + 10;
        }

        /// <summary>
        /// Evaluates the gate connected to it
        /// </summary>
        /// <returns></returns>
        public override bool Evaluate()
        {
            //if inputwire is empty
            if(pins[0].InputWire == null)
            {
                //Do nothing
            }
            else
            {
                //set gateOuput to pin 0 owner
                Gate gateOutput = pins[0].InputWire.FromPin.Owner;
                //if gateoutput is true
                if (gateOutput.Evaluate() == true)
                {
                    Console.WriteLine("Output is on");
                    outputStatus = true; //set output status to true

                }
                //if gateOutput is false
                if (gateOutput.Evaluate() == false)
                {
                    Console.WriteLine("Output is off");
                    outputStatus = false; //set output status to false
                }                
            }
            return outputStatus;



        }

        /// <summary>
        /// Clones the output lamp
        /// </summary>
        /// <param name="g"></param>
        /// <returns>a new output lamp</returns>
        public override Gate Clone(Gate g)
        {
            g = new OutputLamp(0, 0);
            return g;
        }
    }
}
