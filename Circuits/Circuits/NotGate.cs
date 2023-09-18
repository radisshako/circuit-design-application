using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Circuits
{
    public class NotGate : Gate
    {

        /// <summary>
        /// initialises the Not Gate
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public NotGate(int x, int y) : base(x, y)
        {
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, false, 20));
            MoveTo(x, y);
        }

        /// <summary>
        /// Clones the NotGate
        /// </summary>
        /// <param name="g"></param>
        /// <returns>a new NotGate</returns>
        public override Gate Clone(Gate g)
        {
            g = new NotGate(0, 0);
            return g;
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

            //Check if the gate has been selected
            if (selected)
            {
                paper.DrawImage(Properties.Resources.NotGateAllRed, Left, Top); //draw in red
            }
            else
            {
                paper.DrawImage(Properties.Resources.NotGate, Left, Top); //draw in gray
            }

        }

        /// <summary>
        /// Evaluates the NotGate
        /// </summary>
        /// <returns>The opposite of the input</returns>
        public override bool Evaluate()
        {
            //if pin 0 is empty
            if (pins[0].InputWire == null)
            {
                Console.WriteLine("Empty pin");
                //####################################################
                //Not Sure Whether to return false if empty or true
                //####################################################
                return false;
            }
            //set gate A to pin 0 owner
            Gate gateA = pins[0].InputWire.FromPin.Owner;
            //return the opposite of gate A evaluation
            return !gateA.Evaluate();
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
            pins[0].X = x - GAP - 5;
            pins[0].Y = y + GAP + 15;
            pins[1].X = x + GAP + 55;
            pins[1].Y = y + HEIGHT - GAP - 5;
        }
    }
}
