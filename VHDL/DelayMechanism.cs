//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//


namespace VHDL
{

    using Expression = VHDL.expression.Expression;
    using System;
    using System.Runtime.Serialization;

    //
    // * Signal assignment delay mechanism.
    // 
    [Serializable]
    public class TRANSPORTDelayMechanism : DelayMechanism
    { }

    [Serializable]
    public class INERTIALDelayMechanism : DelayMechanism
    { }

    [Serializable]
    public class DUTYCUCLEDelayMechanism : DelayMechanism
    { }

    [Serializable]
    public abstract class DelayMechanism : VhdlElement
    {
        /// <summary>
        /// Transport delay mechanism.
        /// </summary>
        public static TRANSPORTDelayMechanism TRANSPORT = new TRANSPORTDelayMechanism();

        /// <summary>
        /// Inertial delay mechanism.
        /// </summary>
        public static INERTIALDelayMechanism INERTIAL = new INERTIALDelayMechanism();

        /// <summary>
        /// Duty cycle delay mechanism.
        /// </summary>
        public static DUTYCUCLEDelayMechanism DUTY_CYCLE = new DUTYCUCLEDelayMechanism();

        /// <summary>
        /// Creates a reject inertial delay mechanism.
        /// </summary>
        /// <param name="time">the pulse rejection limit</param>
        /// <returns>the created delay mechanism.</returns>
        public static DelayMechanism REJECT_INERTIAL(Expression time)
        {
            return new RejectInertialImpl(time);
        }

        /// <summary>
        /// Returns the pulse rejection limit.
        /// </summary>
        public virtual Expression PulseRejectionLimit
        {
            get { return null; }
        }

        /// <summary>
        /// Prevent subclassing.
        /// </summary>
        public DelayMechanism()
        {
        }

        [Serializable]
        private class RejectInertialImpl : DelayMechanism
        {
            private Expression time;

            /// <summary>
            /// Creates a reject inertial delay mechanism.
            /// </summary>
            /// <param name="time">the puls rejection limit</param>
            public RejectInertialImpl(Expression time)
            {
                this.time = time;
            }

            public override Expression PulseRejectionLimit
            {
                get { return time; }
            }
        }
    }
}