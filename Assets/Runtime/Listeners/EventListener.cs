using UnityEngine;
using UnityEngine.Events;

namespace Railek.Unibase
{
    public abstract class EventListener<TType, TEvent, TResponse> : MonoBehaviour,
        IEventListener<TType> where TEvent : EventBase<TType> where TResponse : UnityEvent<TType>
    {
        [SerializeField] private TEvent @event = default(TEvent);
        [SerializeField] private TEvent previouslyRegisteredEvent = default(TEvent);
        [SerializeField] private TResponse response = default(TResponse);

        private void OnEnable()
        {
            if (@event != null)
            {
                Register();
            }
        }

        private void OnDisable()
        {
            if (@event != null)
            {
                @event.RemoveListener(this);
            }
        }

        public void OnEventRaised(TType value)
        {
            RaiseResponse(value);
        }

        private void RaiseResponse(TType value)
        {
            response.Invoke(value);
        }

        private void Register()
        {
            if (previouslyRegisteredEvent != null)
            {
                previouslyRegisteredEvent.RemoveListener(this);
            }

            @event.AddListener(this);
            previouslyRegisteredEvent = @event;
        }
    }

    public abstract class EventListener<TEvent, TResponse> : MonoBehaviour,
        IEventListener where TEvent : EventBase where TResponse : UnityEvent
    {
        [SerializeField] private TEvent @event = default(TEvent);
        [SerializeField] private TEvent previouslyRegisteredEvent = default(TEvent);
        [SerializeField] private TResponse response = default(TResponse);

        private void OnEnable()
        {
            if (@event != null)
            {
                Register();
            }
        }

        private void OnDisable()
        {
            if (@event != null)
            {
                @event.RemoveListener(this);
            }
        }

        public void OnEventRaised()
        {
            RaiseResponse();
        }

        private void RaiseResponse()
        {
            response.Invoke();
        }

        private void Register()
        {
            if (previouslyRegisteredEvent != null)
            {
                previouslyRegisteredEvent.RemoveListener(this);
            }

            @event.AddListener(this);
            previouslyRegisteredEvent = @event;
        }
    }
}
