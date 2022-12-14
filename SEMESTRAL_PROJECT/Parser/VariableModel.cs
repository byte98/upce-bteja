using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SemestralProject.Parser.Nodes;

namespace SemestralProject.Parser
{
    /// <summary>
    /// Class representing model for variable
    /// </summary>
    public class VariableModel
    {
        /// <summary>
        /// Counter of created variable models
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Name of variable
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Data type of variable
        /// </summary>
        private readonly EDataType type;

        /// <summary>
        /// Value of variable
        /// </summary>
        private IValueNode value = null;

        /// <summary>
        /// Stack with all previous evaluations
        /// </summary>
        private Stack<Value> evaluations;

        /// <summary>
        /// Evaluation of variable
        /// </summary>
        private Value evaluation;

        /// <summary>
        /// Flag, whether variable has been evaluated
        /// </summary>
        private bool evaluated;

        /// <summary>
        /// Identifier of variable model
        /// </summary>
        private readonly long id;

        /// <summary>
        /// Creates new variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="type">Data type of variable</param>
        public VariableModel(string name, EDataType type)
        {
            this.name = name;
            this.type = type;
            this.evaluated = false;
            this.id = VariableModel.counter;
            VariableModel.counter++;
            this.evaluations = new Stack<Value>();
        }

        /// <summary>
        /// Gets name of variable
        /// </summary>
        /// <returns>Name of variable</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Sets value of variable
        /// </summary>
        /// <param name="value">New value of variable</param>
        public void SetValue(IValueNode value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets data type of variable
        /// </summary>
        /// <returns>Data type of variable</returns>
        public EDataType GetDataType()
        {
            return this.type;
        }

        /// <summary>
        /// Checks, whether variable has defined value
        /// </summary>
        /// <returns>TRUE if variable has defined value, FALSE otherwise</returns>
        public bool HasValue()
        {
            return this.value != null;
        }

        /// <summary>
        /// Gets value of variable
        /// </summary>
        /// <returns>Value of variable</returns>
        public IValueNode GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Evaluates variable
        /// </summary>
        public void Evaluate()
        {
            this.value.Execute();
            if (this.value.HasValue())
            {
                this.evaluation = this.value.GetValue();
            }
            this.evaluated = true;
        }

        /// <summary>
        /// Gets evaluation of variable
        /// </summary>
        /// <returns></returns>
        public Value GetEvaluation()
        {
            return this.evaluation;
        }

        /// <summary>
        /// Sets evaluation of variable
        /// </summary>
        /// <param name="value">New value of variable</param>
        public void SetEvaluation(Value value)
        {
            this.evaluation = value;
            this.evaluated = true;
        }

        /// <summary>
        /// Checks, whether variable has been evaluated
        /// </summary>
        /// <returns>TRUE if variable has been evaluated, FALSE otherwise</returns>
        public bool IsEvaluated()
        {
            return this.evaluated;
        }

        /// <summary>
        /// Unevaluates variable
        /// </summary>
        public void Unevaluate()
        {
            this.evaluated = false;
            this.evaluation = null;

        }

        /// <summary>
        /// Gets executable node which produces value of variable
        /// </summary>
        /// <returns>Node whihch produces value of variable</returns>
        public IValueNode GetExecutable()
        {
            return this.value;
        }

        public override string ToString()
        {
            string reti = "VAR#" + this.id + "[" + this.type + "]:" + this.name;
            if (this.IsEvaluated())
            {
                reti += "(";
                reti += this.evaluation.ToString();
                reti += ")";
            }
            return reti;
        }

        /// <summary>
        /// Prepares space for next evaluation
        /// </summary>
        public void NewEvaluation()
        {
            this.evaluations.Push(this.evaluation);
            if (this.evaluation != null)
            {
                if (this.evaluation.GetDataType() == EDataType.FLOAT)
                {
                    this.evaluation = new Value(this.evaluation.GetFloat());
                }
                else if (this.evaluation.GetDataType() == EDataType.INTEGER)
                {
                    this.evaluation = new Value(this.evaluation.GetInt());
                }
                else
                {
                    this.evaluation = new Value(this.evaluation.GetString(), false);
                }
            }
        }

        /// <summary>
        /// Returns state to previous evaluation
        /// </summary>
        public void PreviousEvaluation()
        {
            if (this.evaluations.Count > 0)
            {
                this.evaluation = this.evaluations.Pop();
            }
        }
    }
}
