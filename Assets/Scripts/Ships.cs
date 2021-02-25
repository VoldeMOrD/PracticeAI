using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Ships {
    
    public abstract class Ship : Character {

        public Ship(string name, string resource) : base (name, resource) {
            this._status.Add("speed", 0f);
        }
    }

    public class ShipA : Ship {
        public ShipA(string name = "Ship A") : base (name , "Prefabs/Arc170") {
            this.resizeToMax('x',5);
        }

        public override Bounds GetBounds(){
            Renderer renderer = this.Prefab.transform.Find("ShipBody").GetComponent(typeof(Renderer)) as Renderer;
            if(renderer != null){
                return renderer.bounds;
            }else{
                return new Bounds();
            }
        }
    }

    public class ShipPlayer : ShipA {

        public ShipPlayer() : base () {
            this.AddKeyboardInput();
        }

        protected override void InitKeyboardListeners(){
            KeyboardInput input = this.GetComponent(typeof(KeyboardInput)) as KeyboardInput;
            input.AddKeyDown(KeyCode.A,"Fire");
            input.AddKeyDown(KeyCode.B,"Jump");
            input.AddKeyDown(KeyCode.Space,"Space");
            input.AddKeyUp(KeyCode.W,"Forward");
            input.AddKey(KeyCode.Q,"Side");
        }
    }
}
