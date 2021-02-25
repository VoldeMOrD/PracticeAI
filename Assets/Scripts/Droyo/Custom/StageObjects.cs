using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageObjects {
    
    public abstract class StageObject {

        private string _resource;
        private string _name;
        private GameObject _prefab;

        public StageObject(string name, string resourse) {
            this._resource = resourse;
            this._name = name;
            InitGameObject();
        }

        private void InitGameObject(){
            Object resourse = Resources.Load(this._resource);
            _prefab = GameObject.Instantiate(resourse as GameObject);
            _prefab.name = this._name;
        }

        public void AddComponent(System.Type component){
            if(this.GetComponent(component) == null){
                _prefab.AddComponent(component);
            }
        }

        public GameObject Prefab {
            get {return this._prefab; }
        }
        public Transform Transform {
            get {return this._prefab.transform; }
        }
        
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }

        public void AddComponents(List<System.Type> components){
            foreach(System.Type component in components){
                _prefab.AddComponent(component);
            }
        }

        public Component GetComponent(System.Type type) {
            return this._prefab.GetComponent(type);
        }

        public void AddRigidBody(){
            this.AddComponent(typeof(Rigidbody));
        }

        public virtual Bounds GetBounds(){
            Renderer renderer = this.GetComponent(typeof(Renderer)) as Renderer;
            if(renderer != null){
                return renderer.bounds;
            }else{
                return new Bounds();
            }
        }

        public void resizeToMax(char axis, float max){
            Bounds bounds = this.GetBounds();
            float actualSize = 0f;
            float actualScale = 0f;
            switch(axis){
                case 'x': 
                    actualSize = bounds.extents.x; 
                    actualScale = this.Transform.localScale.x; 
                    break;
                case 'y': 
                    actualSize = bounds.extents.y;
                    actualScale = this.Transform.localScale.y; 
                    break;
                default : 
                    actualSize = bounds.extents.z;
                    actualScale = this.Transform.localScale.x; 
                    break;
            }
            float newScale = (actualScale * max) / actualSize;
            Vector3 newVectorScale = new Vector3(newScale, newScale, newScale);
            this.Transform.localScale = newVectorScale;
        }
    }
}
