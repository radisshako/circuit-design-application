using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Circuits
{
    //##############################################################
    //Questions:
    //1. Is it a better idea to fully document the Gate class or the AndGate
    //subclass? Can you inherit comments?

    //xml comments are inheritable but it would still be a good idea to do inline comments in the
    //individual classes


    //2. What is the advantage of making a method abstract in the superclass
    //rather than just writing a virtual method with no code in the body of
    //the method? Is there any disadvantage to an abstract method?

    //Using an abstract class means that you dont have to cast objects to use the correct method
    //virtual methods are better when the code is repeated in subclasses
    //abstract methods reduce repeated code
    //Disadvantage: You have to override every abstract method in every new class


    //3. If a class has an abstract method in it, does the class have to be
    //abstract?

    //Yes.


    //4. What would happen in your program if one of the gates added to your
    //Compound Gate is another Compound Gate? Is your design robust
    //enough to cope with this situation?

    //Yes, if you click on the larger compound it moves the whole thing.
    //if you click on the smaller compound within the larger compound it moves only
    //the smaller compound

    /// <summary>
    /// The main GUI for the COMP104 digital circuits editor.
    /// This has a toolbar, containing buttons called buttonAnd, buttonOr, etc.
    /// The contents of the circuit are drawn directly onto the form.
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The (x,y) mouse position of the last MouseDown event.
        /// </summary>
        protected int startX, startY;

        /// <summary>
        /// If this is non-null, we are inserting a wire by
        /// dragging the mouse from startPin to some output Pin.
        /// </summary>
        protected Pin startPin = null;

        /// <summary>
        /// The (x,y) position of the current gate, just before we started dragging it.
        /// </summary>
        protected int currentX, currentY;

        /// <summary>
        /// The set of gates in the circuit
        /// </summary>
        protected List<Gate> gatesList = new List<Gate>();

        /// <summary>
        /// The set of connector wires in the circuit
        /// </summary>
        protected List<Wire> wiresList = new List<Wire>();

        /// <summary>
        /// The currently selected gate, or null if no gate is selected.
        /// </summary>
        protected Gate current = null;

        /// <summary>
        /// The new gate that is about to be inserted into the circuit
        /// </summary>
        protected Gate newGate = null;

        /// <summary>
        /// The new compound that is about to be inserted into the circuit
        /// </summary>
        protected Compound newCompound = null;






        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Handles all events when a mouse is clicked in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //if current is not null
            if (current != null)
            {
                //set current gate selected  to false
                current.Selected = false;
                //if current is a compound gate
                if (current is Compound c)
                {
                    //for each gate within current compound gate
                    foreach (Gate j in ((Compound)current).CompoundGateList)
                    {
                        //set gate selected to false
                        j.Selected = false;
                    }
                }
                //set current to null
                current = null;
                //redraw the screen
                this.Invalidate();

            }

            // See if we are inserting a new gate
            if (newGate != null)
            {
                newGate.MoveTo(e.X, e.Y);
                gatesList.Add(newGate);
                newGate = null;
            }
            else
            {
                // search for the first gate under the mouse position
                foreach (Gate g in gatesList)
                {
                    //if mouse x and y is within a gate
                    if (g.IsMouseOn(e.X, e.Y))
                    {
                        //set gate selected to true
                        g.Selected = true;
                        //set current to g
                        current = g;

                        //if gate is inputsource
                        if (g is InputSource)
                        {
                            //change input source voltage
                            ((InputSource)g).ChangeSelected();
                        }
                        //if creating a new compound
                        if (newCompound != null)
                        {
                            //add current gate to the new compound
                            newCompound.AddGate(current);
                            //for each gate within the new compound
                            foreach (Gate l in newCompound.CompoundGateList)
                            {
                                //set the gate seleced to true
                                l.Selected = true;
                            }
                        }
                        //redraw the screen
                        this.Invalidate();
                    }
                    //if gate is a compound
                    if (g is Compound c)
                    {
                        //if mouse x and y postion is within a compound gate
                        if (c.compoundisMouseOn(e.X, e.Y) == true)
                        {
                            //for each gate within the compound gate 
                            foreach (Gate j in ((Compound)c).CompoundGateList)
                            {
                                //set gate selected to true
                                j.Selected = true;
                            }
                            //set current to gate
                            current = g;
                            //redraw the screen
                            this.Invalidate();
                            
                        }

                    }
                }
            }
            //redraw the screen
            this.Invalidate();
        }

        /// <summary>
        /// Handles events while the mouse button is pressed down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //if current is null/empty
            if (current == null)
            {
                // try to start adding a wire
                startPin = findPin(e.X, e.Y);
            }
            //if mouse x and y postion is within current gate
            else if (current.IsMouseOn(e.X, e.Y))
            {
                // start dragging the current object around
                startX = e.X;
                startY = e.Y;
                currentX = current.Left;
                currentY = current.Top;
            }
            //if current is a compound gate
            else if (current is Compound c)
            {
                //if mouse x and y postion is within compound gate
                if (c.compoundisMouseOn(e.X, e.Y) == true)
                {
                    // start dragging the current object around
                    startX = e.X;
                    startY = e.Y;
                    currentX = c.Left + c.RefPoint.X;
                    currentY = c.Top + c.RefPoint.Y;
                }
            }
        }

        /// <summary>
        /// Handles all events when the mouse is moving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //if start pin is not null
            if (startPin != null)
            {
                Console.WriteLine("wire from " + startPin + " to " + e.X + "," + e.Y);
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate();  // this will draw the line
            }
            //if start X and start Y is greater or equal to 0 and current has a gate
            else if (startX >= 0 && startY >= 0 && current != null)
            {
                //move the gate to the mouse position
                Console.WriteLine("mouse move to " + e.X + "," + e.Y);
                Console.WriteLine("outside" + current);
                current.MoveTo(currentX + (e.X - startX), currentY + (e.Y - startY));
                this.Invalidate(); //draw the new change
            }
            //if new gate is not null
            else if (newGate != null)
            {
                //move new gate to the mouse position
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate(); //redraw the screen
            }
        }

        /// <summary>
        /// Handles all events when the mouse button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (startPin != null)
            {
                // see if we can insert a wire
                Pin endPin = findPin(e.X, e.Y);
                if (endPin != null)
                {
                    Console.WriteLine("Trying to connect " + startPin + " to " + endPin);
                    Pin input, output;
                    if (startPin.IsOutput)
                    {
                        input = endPin;
                        output = startPin;
                    }
                    else
                    {
                        input = startPin;
                        output = endPin;
                    }
                    if (input.IsInput && output.IsOutput)
                    {
                        if (input.InputWire == null)
                        {
                            Wire newWire = new Wire(output, input);
                            input.InputWire = newWire;
                            wiresList.Add(newWire);
                        }
                        else
                        {
                            MessageBox.Show("That input is already used.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: you must connect an output pin to an input pin.");
                    }
                }
                startPin = null;
                this.Invalidate();
            }
            // We have finished moving/dragging
            startX = -1;
            startY = -1;
            currentX = 0;
            currentY = 0;
        }

        /// <summary>
        /// This will create a new and gate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAnd_Click(object sender, EventArgs e)
        {
            newGate = new AndGate(0, 0);
        }

        /// <summary>
        /// this will create a new or gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newGate = new OrGate(0, 0);
        }

        /// <summary>
        /// this will create a new not gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonNot_Click(object sender, EventArgs e)
        {
            newGate = new NotGate(0, 0);
        }

        /// <summary>
        /// this will create a new input source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonInput_Click(object sender, EventArgs e)
        {
            newGate = new InputSource(0, 0);
        }

        /// <summary>
        /// this will create a new output lamp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOutputLamp_Click(object sender, EventArgs e)
        {
            newGate = new OutputLamp(0, 0);
        }

        /// <summary>
        /// For each gate on the screen/list call their evaluate method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //for each gate in the gate list
            foreach (Gate gates in gatesList)
            {
                //call their respective evaluate method
                gates.Evaluate();
                Console.WriteLine(gates.Evaluate().ToString());
                
            }
            this.Invalidate(); //redraw the screen
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Ignore
        }

        private void toolStripButtonClone_Click(object sender, EventArgs e)
        {
            //if current is null ignore
            if (current == null) { } //do nothing
            //if clone button is pressed
            else if (current.CloneSelected == true) { current.CloneSelected = false; } //set button status to false
            else { current.CloneSelected = true; } //set button statsus to true

            //if current is null ignore
            if (current == null) { } //do nothing
            //if a gate is selected and clone button is pressed
            else if (current.Selected == true && current.CloneSelected == true)
            {
                newGate = current.Clone(newGate); //create copy of gate
            }
        }

        /// <summary>
        /// Create a new compound when start group button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonStartGroup_Click(object sender, EventArgs e)
        {
            //create a new empty compound gate
            newCompound = new Compound(0, 0);
        }

        /// <summary>
        /// Adds the new compound to new gate and creates the compound gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonEndGroup_Click(object sender, EventArgs e)
        {
            //if new compound was created
            if (newCompound != null)
            {
                //for each gate within the compound gate
                foreach (Gate l in newCompound.CompoundGateList)
                {
                    l.Selected = false; //set gate selected to false
                }
                newGate = newCompound; //set the new compound as a new gate
                newCompound = null; //clear the new compound
            }
        }




        /// <summary>
        /// Finds the pin that is close to (x,y), or returns
        /// null if there are no pins close to the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Pin findPin(int x, int y)
        {
            foreach (Gate g in gatesList)
            {
                foreach (Pin p in g.Pins)
                {
                    if (p.isMouseOn(x, y))
                        return p;
                }
            }
            return null;
        }

        /// <summary>
        /// Redraws all the graphics for the current circuit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Draw all of the gates
            foreach (Gate g in gatesList)
            {
                g.Draw(e.Graphics);
            }
            //Draw all of the wires
            foreach (Wire w in wiresList)
            {
                w.Draw(e.Graphics);
            }
            //Draw all the wires
            if (startPin != null)
            {
                e.Graphics.DrawLine(Pens.White,
                    startPin.X, startPin.Y,
                    currentX, currentY);
            }
            if (newGate != null)
            {
                // show the gate that we are dragging into the circuit
                newGate.MoveTo(currentX, currentY);
                newGate.Draw(e.Graphics);
            }
        }

        
    }
}
