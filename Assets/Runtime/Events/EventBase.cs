using System.Collections.Generic;
using UnityEngine;
using System;

namespace Railek.Unibase
{
    public abstract class EventBase<T> : EventBase
    {
        private readonly List<IEventListener<T>> _listeners = new List<IEventListener<T>>();
        private readonly List<Action<T>> _actions = new List<Action<T>>();

        [SerializeField] protected T debugValue;

        public void Raise(T value)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
            }

            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }

            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                _actions[i](value);
            }

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                actions[i]();
            }
        }

        public void AddListener(IEventListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        public void AddListener(Action<T> action)
        {
            if (!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }

        public void RemoveListener(Action<T> action)
        {
            if (_actions.Contains(action))
            {
                _actions.Remove(action);
            }
        }

        public override string ToString()
        {
            return "EventBase<" + typeof(T) + ">";
        }
    }

    public abstract class EventBase : ScriptableObject
    {
        protected readonly List<IEventListener> listeners = new List<IEventListener>();
        protected readonly List<Action> actions = new List<Action>();

        public void Raise()
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                actions[i]();
            }
        }

        public void AddListener(IEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public void AddListener(Action action)
        {
            if (!actions.Contains(action))
            {
                actions.Add(action);
            }
        }

        public void RemoveListener(Action action)
        {
            if (actions.Contains(action))
            {
                actions.Remove(action);
            }
        }

        public virtual void RemoveAll()
        {
            listeners.RemoveRange(0, listeners.Count);
            actions.RemoveRange(0, actions.Count);
        }
    }
}
