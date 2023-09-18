using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuits
{
    public class OrGate : Gate
    {
        /// <summary>
        /// Initialises the Or Gate
        /// </summary>
        /// <param name="x">mouse x postion</param>
        /// <param name="y">mouse y postion</param>
        public OrGate(int x, int y) : base(x, y)
        {
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, false, 20));
            MoveTo(x, y);
        }

        /// <summary>
        /// Clones the OrGate
        /// </summary>
        /// <param name="g"></param>
        /// <returns>a new OrGate</returns>
        public override Gate Clone(Gate g)
        {
            g = new OrGate(0, 0);
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
                paper.DrawImage(Properties.Resources.OrGateAllRed, Left, Top); //draw in red
            }
            else
            {
                paper.DrawImage(Properties.Resources.OrGate, Left, Top); //draw in grey
            }

        }

        /// <summary>
        /// Evaluates the OrGate
        /// </summary>
        /// <returns>Whether the gate is true or false</returns>
        public override bool Evaluate()
        {
            //if pin 0 owner and pin 1 owner is empty return false
            if (pins[0].InputWire == null || pins[1].InputWire == null)
            {
                Console.WriteLine("Empty pin");
                return false;
            }
            //set Gate A to pin 0 owner
            Gate gateA = pins[0].InputWire.FromPin.Owner;
            //set Gate B to pin 1 owner
            Gate gateB = pins[1].InputWire.FromPin.Owner;

            //if Gate A or Gate B is true return true, else false
            if (gateA.Evaluate() == true || gateB.Evaluate() == true)
            {
                return true;
            }
            else
            {
                return false;
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
            pins[0].X = x - GAP;
            pins[0].Y = y + GAP;
            pins[1].X = x - GAP;
            pins[1].Y = y + HEIGHT - GAP + 10;
            pins[2].X = x + WIDTH + GAP + 30;
            pins[2].Y = (y + HEIGHT / 2) + 5;
        }
    }
}
