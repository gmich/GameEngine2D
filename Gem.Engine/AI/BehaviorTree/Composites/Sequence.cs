﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Iterates BehaviorNodes terminates when a node succeeds. Behaves like the logical OR operator
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Sequence<AIContext> : IBehaviorNode<AIContext>
    {
        private readonly Stack<IBehaviorNode<AIContext>> pendingNodes;
        private readonly IBehaviorNode<AIContext>[] nodes;
        private BehaviorResult behaviorResult;
        public event EventHandler OnBehaved;

        public Sequence(IBehaviorNode<AIContext>[] nodes)
        {
            this.nodes = nodes;
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes);
        }

        private bool HasProcessedAllNodes => pendingNodes.Count == 0;

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { return nodes; } }

        public BehaviorResult Behave(AIContext context)
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs());
            if (HasProcessedAllNodes)
            {
                return behaviorResult;
            }

            var currentNode = pendingNodes.Pop();

            switch (behaviorResult = currentNode.Behave(context))
            {
                case BehaviorResult.Running:
                    //reevaluate the next iteration
                    pendingNodes.Push(currentNode);
                    break;
                case BehaviorResult.Failure:
                    //stop iterating nodes
                    pendingNodes.Clear();
                    break;
            }
            return BehaviorResult.Running;
        }
    }
}
