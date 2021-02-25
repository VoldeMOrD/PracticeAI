using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageObjects;
using System.Reflection;
using System;
using UnityEngine.Events;

namespace Characters {
    
    public abstract class Character : StageObject {

        protected Dictionary<string, float> _status = new Dictionary<string, float>();
        private int _lvl;
        private Dictionary<int, CharacterModificator> _modificator =  new Dictionary<int, CharacterModificator>();
        public Character (string name, string resource) : base (name, resource) {
            this._status.Add("hp", 0f);
            this._status.Add("base_hp", 0f);
            this.AddComponent(typeof(CharacterScript));
            this.Prefab.GetComponent<CharacterScript>().SetCharacter(this);
        }

        public float Status(string key) {
            if(this._modificator.ContainsKey(this._lvl) && this._modificator[this._lvl].GetModificator(key) != 0){
                return this._status[key] * this._modificator[this._lvl].GetModificator(key);
            }
            return this._status[key];
        }

        public Dictionary<string, float> Status(){
            return this._status;
        }

        public int LVL {
            get { return this._lvl; }
        }

        public virtual int LVLUp(){
            this._lvl++;
            return this._lvl;
        }

        public CharacterModificator Modificator {
            set { this._modificator[this._lvl] = value; }
        }

        public void AddKeyboardInput(){
            this.AddComponent(typeof(EventManager));
            this.AddComponent(typeof(KeyboardInput));
            InitKeyboardListeners();
            this.AddComponent(typeof(PlayerInput));
        }

        protected virtual void InitKeyboardListeners(){
            KeyboardInput input = this.GetComponent(typeof(KeyboardInput)) as KeyboardInput;
        }

        public virtual void InputEventCatcher(string e){
            Debug.Log("Event catch! code: " + e);
        }
    }

    public class CharacterScript : MonoBehaviour {
        
        private Character _character;

        public void SetCharacter(Character character) {
            if(this._character == null)
                this._character = character;
        }

        public Character GetCharacter() {
            return this._character;
        }

        public float Status(string key){
            return this.GetCharacter().Status(key);
        }
        
    }

    public class PlayerInput : MonoBehaviour {

        private Dictionary<string,UnityAction> _listeners = new Dictionary<string, UnityAction>();
        
        private void Awake() {
            if(this.GetComponent<KeyboardInput>() != null){
                this.Init();
            }else{
                Debug.LogError("There needs to be add the KeyboardInput in the scene.");
            }
        }

        private void Init(){
            foreach(KeyValuePair<KeyCode, string> key in this.GetComponent<KeyboardInput>().GetKeyDowns()){
                _listeners.Add(key.Value, new UnityAction(delegate(){
                    this.GetComponent<CharacterScript>().GetCharacter().InputEventCatcher(key.Value);
                }));
                EventManager.StartListening(key.Value, _listeners[key.Value]);    
            }

            foreach(KeyValuePair<KeyCode, string> key in this.GetComponent<KeyboardInput>().GetKeyUps()){
                _listeners.Add(key.Value, new UnityAction(delegate(){
                    this.GetComponent<CharacterScript>().GetCharacter().InputEventCatcher(key.Value);
                }));
                EventManager.StartListening(key.Value, _listeners[key.Value]);    
            }

            foreach(KeyValuePair<KeyCode, string> key in this.GetComponent<KeyboardInput>().GetKey()){
                _listeners.Add(key.Value, new UnityAction(delegate(){
                    this.GetComponent<CharacterScript>().GetCharacter().InputEventCatcher(key.Value);
                }));
                EventManager.StartListening(key.Value, _listeners[key.Value]);    
            }
        }

        private void OnEnable() {
            foreach(KeyValuePair<string,UnityAction> listener in _listeners){
                EventManager.StartListening(listener.Key, listener.Value);    
            }
        }

        private void OnDisable() {
            foreach(KeyValuePair<string,UnityAction> listener in _listeners){
                EventManager.StopListening(listener.Key, listener.Value);    
            }
        }
    }
    public class KeyboardInput : MonoBehaviour {
    
        Dictionary<KeyCode, string> _keysDown = new Dictionary<KeyCode, string>();
        Dictionary<KeyCode, string> _keysUp = new Dictionary<KeyCode, string>();
        Dictionary<KeyCode, string> _keys = new Dictionary<KeyCode, string>();

        private void Awake() {
            EventManager eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;
            if (!eventManager) {
                Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
            }
        }

        public void OnGUI(){
            if(_keysDown.Count > 0) {
                foreach(KeyValuePair<KeyCode, string> result in _keysDown){
                    if (Input.GetKeyDown(result.Key)){
                        EventManager.TriggerEvent(result.Value);
                    }
                }
            }

            if(_keysUp.Count > 0) {
                foreach(KeyValuePair<KeyCode, string> result in _keysUp){
                    if (Input.GetKeyUp(result.Key)){
                        EventManager.TriggerEvent(result.Value);
                    }
                }
            }

            if(_keys.Count > 0) {
                foreach(KeyValuePair<KeyCode, string> result in _keys){
                    if (Input.GetKey(result.Key)){
                        EventManager.TriggerEvent(result.Value);
                    }
                }
            }
        }

        private void FixedUpdate() {
            this.OnGUI();
        }

        public void AddKeyDown(KeyCode key, string eventName){
            _keysDown.Add(key, eventName);
        }
        public void AddKeyUp(KeyCode key, string eventName){
            _keysUp.Add(key, eventName);
        }
        public void AddKey(KeyCode key, string eventName){
            _keys.Add(key, eventName);
        }

        public Dictionary<KeyCode, string> GetKeyDowns(){
            return _keysDown;
        }

        public Dictionary<KeyCode, string> GetKeyUps(){
            return _keysUp;
        }
        public Dictionary<KeyCode, string> GetKey(){
            return _keys;
        }
    }

    public class CharacterModificator {

        protected Type _target;
        private Dictionary<string, float> _modificators = new Dictionary<string, float>();

        public CharacterModificator (Dictionary<string, float> modificators){
            this._modificators = modificators;
        }

        public void SetModificator(string key, float value){
            this._modificators[key] = value;
        }

        public float GetModificator(string key){
            if(this._modificators.ContainsKey(key)){
                return this._modificators[key];
            }
            return 0f;
        }
    }
}
