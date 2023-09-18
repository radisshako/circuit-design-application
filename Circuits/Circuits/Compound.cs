using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuits
{
    public class Compound : Gate
    {
        //List of gate objects
        private List<Gate> compoundGateList;

        /// <summary>
        /// reference point for the offsets (top left)
        /// </summary>
        private Point refPoint;

        /// <summary>
        /// Initilise the compound gate class
        /// </summary>
        /// <param name="x">the x postion</param>
        /// <param name="y">the y position</param>
        public Compound(int x, int y) : base(x, y)
        {
            compoundGateList = new List<Gate>();
        }

        /// <summary>
        /// Gets the reference point (top/left)
        /// </summary>
        public Point RefPoint
        {
            get { return refPoint; }
        }
        /// <summary>
        /// gets the list of compounds
        /// </summary>
        public List<Gate> CompoundGateList
        {
            get { return compoundGateList; }
        }


        /// <summary>
        /// checks whether the mouse position is within a compound gate
        /// </summary>
        /// <param name="x">mouse x position</param>
        /// <param name="y">mouse y position</param>
        /// <returns></returns>
        public bool compoundisMouseOn(int x, int y)
        {
            //for each gate in the compound
            foreach (Gate gate in compoundGateList)
            {
                //return true if mouse is within gate
                if (gate.IsMouseOn(x, y))
                {
                    return true;
                }
            }
            //else return false
            return false;
        }


        /// <summary>
        /// Adds a gate to the compound gate list
        /// </summary>
        /// <param name="g"></param>
        public void AddGate(Gate g)
        {
            compoundGateList.Add(g);    
        }

        /// <summary>
        /// Clones the compound gate
        /// </summary>
        /// <param name="newGate">the new gate to be created</param>
        /// <returns>Compound gate</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Gate Clone(Gate newGate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws all the gates within the compound
        /// </summary>
        /// <param name="paper">The drawing object</param>
        public override void Draw(Graphics paper)
        {
            //for each gate within the compound
            foreach(Gate g in compoundGateList)
            {
                //draw the gate
                g.Draw(paper);
            }
        }

        /// <summary>
        /// Evaluates the individuals gates within compound
        /// </summary>
        /// <returns>the evaluation</returns>
        public override bool Evaluate()
        {
            //for each gate in the compound
            foreach(Gate gate in compoundGateList)
            {
                //call their respective evaluate method
                gate.Evaluate();
            }
            //else return false
            return false;
        }

        /// <summary>
        /// Moves the compound gate
        /// </summary>
        /// <param name="x">mouse x position</param>
        /// <param name="y">mouse y position</param>
        public override void MoveTo(int x, int y)
        {
            int leftIndex = 0; //index of the left most gate
            int topIndex = 0; //index of the top most gate
            int xOff = 0; //x offset
            int yOff = 0; //y offset

            //Finding the leftmost gate
            for (int i = 1; i < compoundGateList.Count; i++)
            {
                //if gate after is more left than current
                if (compoundGateList[i].Left < compoundGateList[leftIndex].Left)
                {
                    //set the new minimum x index
                    leftIndex = i;

                }

            }
            //finding the topmost index
            for(int j = 1; j < compoundGateList.Count; j++)
            {
                //if gate after is higher than current
                if (compoundGateList[j].Top < compoundGateList[topIndex].Top)
                {
                    //set the new topmost index
                    topIndex = j;
                }
            }
            //if compound gate list is not empty
            if (compoundGateList.Count > 0)
            {
                //create a new point at the position of leftmost and top most x and y
                refPoint = new Point(compoundGateList[leftIndex].Left, compoundGateList[topIndex].Top);

                //moving the gates according to their distance from the new point
                for (int k = 0; k < compoundGateList.Count; k++)
                {
                    xOff = compoundGateList[k].Left - refPoint.X;
                    yOff = compoundGateList[k].Top - refPoint.Y;
                    //move compound gate to mouse x and y with the offsets
                    compoundGateList[k].MoveTo(xOff + x, yOff + y);
                }
            }
        }
    }
}
