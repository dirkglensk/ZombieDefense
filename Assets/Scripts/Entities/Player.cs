    using Interfaces;
    using UnityEngine;

    public class Player : MonoBehaviour, ICharacter
    {
        public ICharacter GetCharacter()
        {
            return this;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
