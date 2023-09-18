using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Circuits
{
    /// <summary>
    /// This class implements an AND gate with two inputs
    /// and one output.
    /// </summary>
    public class AndGate : Gate
    {

        /// <summary>
        /// Initialises the Gate.
        /// </summary>
        /// <param name="x">The x position of the gate</param>
        /// <param name="y">The y position of the gate</param>
        public AndGate(int x, int y) : base(x, y)
        {
            //Add the two input pins to the gate
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, true, 20));
            //Add the output pin to the gate
            pins.Add(new Pin(this, false, 20));
            //move the gate and the pins to the position passed in
            MoveTo(x, y); 
        }

        /// <summary>
        /// clones the current gate
        /// </summary>
        /// <param name="g"></param>
        /// <returns>a new AndGate</returns>
        public override Gate Clone(Gate g)
        {
            g = new AndGate(0, 0);
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
                //draw the gate in red
                paper.DrawImage(Properties.Resources.AndGateAllRed, Left, Top);
            }
            else
            {
                //draw the gate in gray
                paper.DrawImage(Properties.Resources.AndGate, Left, Top);
            }
            

        }

        /// <summary>
        /// Evaluates the AndGate
        /// </summary>
        /// <returns>Whether the gate is true or false</returns>
        public override bool Evaluate()
        {
            //if the pins are empty return false
            if (pins[0].InputWire == null || pins[1].InputWire == null)
            {
                return false;
            }
            
            Gate gateA = pins[0].InputWire.FromPin.Owner; //set gate A to pin 0 owner
            Gate gateB = pins[1].InputWire.FromPin.Owner; //set gate B to pin 1 owner
            
            //if gate A and gate B is true
            if(gateA.Evaluate() == true && gateB.Evaluate() == true)
            {
                return true; //return true
            }
            else
            {
                return false; //return false
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
            pins[2].X = x + WIDTH + GAP + 15;
            pins[2].Y = (y + HEIGHT / 2) + 5;
        }
    }
}
