using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Modding;
using SFCore.Utils;

namespace ImmortalLight
{
    public static class FsmUtil
    {
        public static FsmState AddFsmState(this PlayMakerFSM fsm, string stateName)
        {
            List<FsmState> list = fsm.Fsm.States.ToList();
            FsmState fsmState = new FsmState(fsm.Fsm);
            list.Add(fsmState);
            fsm.Fsm.States = list.ToArray();
            return fsmState;
        }

        public static FsmEvent AddTransition(this PlayMakerFSM fsm, string stateName, string eventName, string toState)
        {
            FsmState state = fsm.Fsm.GetState(stateName);
            List<FsmTransition> list = state.Transitions.ToList();
            FsmEvent fsmEvent = FsmEvent.GetFsmEvent(eventName);
            list.Add(new FsmTransition
            {
                ToState = toState,
                FsmEvent = fsmEvent
            });
            state.Transitions = list.ToArray();
            return fsmEvent;
        }

        public static FsmEvent AddGlobalTransition(this PlayMakerFSM fsm, string globalEventName, string toState)
        {
            List<FsmTransition> list = new List<FsmTransition>(fsm.Fsm.GlobalTransitions);
            FsmEvent fsmEvent = new FsmEvent(globalEventName)
            {
                IsGlobal = true
            };
            list.Add(new FsmTransition
            {
                ToState = toState,
                FsmEvent = fsmEvent
            });
            fsm.Fsm.GlobalTransitions = list.ToArray();
            return fsmEvent;
        }

        public static void ChangeTransition(this PlayMakerFSM fsm, string stateName, string eventName, string toState)
        {
            FsmState state = fsm.Fsm.GetState(stateName);
            FsmTransition fsmTransition = state.Transitions.First((FsmTransition t) => t.EventName.Equals(eventName));
            fsmTransition.ToState = toState;
        }

        public static FsmState GetState(this PlayMakerFSM fsm, string stateName)
        {
            return fsm.Fsm.GetState(stateName);
        }

        public static FsmState CopyState(this PlayMakerFSM fsm, string fromState, string toState)
        {
            FsmState fsmState = new FsmState(fsm.Fsm.GetState(fromState))
            {
                Name = toState
            };
            List<FsmState> list = new List<FsmState>(fsm.FsmStates);
            list.Add(fsmState);
            fsm.Fsm.States = list.ToArray();
            return fsmState;
        }

        public static TAction GetAction<TAction>(this PlayMakerFSM self, string state, int index) where TAction : FsmStateAction
        {
            return (TAction)self.FsmStates.First((FsmState s) => s.Name.Equals(state)).Actions[index];
        }
        public static TAction GetAction<TAction>(this FsmState self, int index) where TAction : FsmStateAction
        {
            return (TAction)self.Actions[index];
        }
        public static void AddAction(this PlayMakerFSM self, string state, FsmStateAction action)
        {
            FsmState fsmState = self.FsmStates.First((FsmState s) => s.Name.Equals(state));
            List<FsmStateAction> list = new List<FsmStateAction>(fsmState.Actions);
            list.Add(action);
            fsmState.Actions = list.ToArray();
        }

        public static void InsertAction(this PlayMakerFSM self, string state, FsmStateAction action, int index)
        {
            FsmState fsmState = self.FsmStates.First((FsmState s) => s.Name.Equals(state));
            List<FsmStateAction> list = new List<FsmStateAction>(fsmState.Actions);
            list.Insert(index, action);
            fsmState.Actions = list.ToArray();
        }
        public static void InsertAction(this FsmState self, FsmStateAction action, int index)
        {
            List<FsmStateAction> list = new List<FsmStateAction>(self.Actions);
            list.Insert(index, action);
            self.Actions = list.ToArray();
        }

        public static void RemoveAction(this PlayMakerFSM self, string state, int index)
        {
            FsmState fsmState = self.FsmStates.First((FsmState s) => s.Name.Equals(state));
            List<FsmStateAction> list = new List<FsmStateAction>(fsmState.Actions);
            list.RemoveAt(index);
            fsmState.Actions = list.ToArray();
        }
        public static void RemoveAction(this FsmState self, int index)
        {
            List<FsmStateAction> list = new List<FsmStateAction>(self.Actions);
            list.RemoveAt(index);
            self.Actions = list.ToArray();
        }

        public static void RemoveTransition(this PlayMakerFSM fsm, string fromStateName, string onEventName)
        {
            fsm.Fsm.GetState(fromStateName).RemoveTransition(onEventName);
        }
        public static void RemoveTransition(this FsmState state, string onEventName)
        {
            FsmTransition[] transitions = state.Transitions;
            FsmTransition[] array = new FsmTransition[transitions.Length - 1];
            int i = 0;
            int num = 0;
            for (; i < transitions.Length; i++)
            {
                if (transitions[i].EventName != onEventName)
                {
                    array[num] = transitions[i];
                    num++;
                }
            }

            state.Transitions = array;
        }


        public static void AddMethod(this PlayMakerFSM self, string state, Action method)
        {
            self.AddAction(state, new MethodAction
            {
                method = method
            });
        }

        public static void InsertMethod(this PlayMakerFSM self, string state, Action method, int index)
        {
            self.InsertAction(state, new MethodAction
            {
                method = method
            }, index);
        }
        public static void InsertMethod(this FsmState self, Action method, int index)
        {
            
            self.InsertAction( new MethodAction
            {
                method = method
            }, index);
        }

        public static void AddFloatVariable(this PlayMakerFSM self, string name)
        {
            List<FsmFloat> list = new List<FsmFloat>(self.FsmVariables.FloatVariables);
            list.Add(new FsmFloat(name));
            self.FsmVariables.FloatVariables = list.ToArray();
        }

        public static void AddIntVariable(this PlayMakerFSM self, string name)
        {
            List<FsmInt> list = new List<FsmInt>(self.FsmVariables.IntVariables);
            list.Add(new FsmInt(name));
            self.FsmVariables.IntVariables = list.ToArray();
        }

        public static void AddBoolVariable(this PlayMakerFSM self, string name)
        {
            List<FsmBool> list = new List<FsmBool>(self.FsmVariables.BoolVariables);
            list.Add(new FsmBool(name));
            self.FsmVariables.BoolVariables = list.ToArray();
        }

        public static void AddStringVariable(this PlayMakerFSM self, string name)
        {
            List<FsmString> list = new List<FsmString>(self.FsmVariables.StringVariables);
            list.Add(new FsmString(name));
            self.FsmVariables.StringVariables = list.ToArray();
        }

        public static void AddVector2Variable(this PlayMakerFSM self, string name)
        {
            List<FsmVector2> list = new List<FsmVector2>(self.FsmVariables.Vector2Variables);
            list.Add(new FsmVector2(name));
            self.FsmVariables.Vector2Variables = list.ToArray();
        }

        public static void AddVector3Variable(this PlayMakerFSM self, string name)
        {
            List<FsmVector3> list = new List<FsmVector3>(self.FsmVariables.Vector3Variables);
            list.Add(new FsmVector3(name));
            self.FsmVariables.Vector3Variables = list.ToArray();
        }

        public static void AddColorVariable(this PlayMakerFSM self, string name)
        {
            List<FsmColor> list = new List<FsmColor>(self.FsmVariables.ColorVariables);
            list.Add(new FsmColor(name));
            self.FsmVariables.ColorVariables = list.ToArray();
        }

        public static void AddRectVariable(this PlayMakerFSM self, string name)
        {
            List<FsmRect> list = new List<FsmRect>(self.FsmVariables.RectVariables);
            list.Add(new FsmRect(name));
            self.FsmVariables.RectVariables = list.ToArray();
        }

        public static void AddQuaternionVariable(this PlayMakerFSM self, string name)
        {
            List<FsmQuaternion> list = new List<FsmQuaternion>(self.FsmVariables.QuaternionVariables);
            list.Add(new FsmQuaternion(name));
            self.FsmVariables.QuaternionVariables = list.ToArray();
        }

        public static void AddGameObjectVariable(this PlayMakerFSM self, string name)
        {
            List<FsmGameObject> list = new List<FsmGameObject>(self.FsmVariables.GameObjectVariables);
            list.Add(new FsmGameObject(name));
            self.FsmVariables.GameObjectVariables = list.ToArray();
        }

        public static void MakeLog(this PlayMakerFSM self, bool additionalLogging = false)
        {
            FsmState[] fsmStates = self.FsmStates;
            foreach (FsmState fsmState in fsmStates)
            {
                for (int num = fsmState.Actions.Length - 1; num >= 0; num--)
                {
                    self.InsertAction(fsmState.Name, new StatusLog
                    {
                        text = $"{num} ('{fsmState.Actions[num].Name}')"
                    }, num);
                    if (additionalLogging)
                    {
                        self.InsertMethod(fsmState.Name, delegate
                        {
                            FsmVariables fsmVariables = self.FsmVariables;
                            FsmFloat[] floatVariables = fsmVariables.FloatVariables;
                            foreach (FsmFloat fsmFloat in floatVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[FloatVariables] - '{fsmFloat.Name}': '{fsmFloat.Value}'");
                            }

                            FsmInt[] intVariables = fsmVariables.IntVariables;
                            foreach (FsmInt fsmInt in intVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[IntVariables] - '{fsmInt.Name}': '{fsmInt.Value}'");
                            }

                            FsmBool[] boolVariables = fsmVariables.BoolVariables;
                            foreach (FsmBool fsmBool in boolVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[BoolVariables] - '{fsmBool.Name}': '{fsmBool.Value}'");
                            }

                            FsmString[] stringVariables = fsmVariables.StringVariables;
                            foreach (FsmString fsmString in stringVariables)
                            {
                                Logger.Log("[" + self.gameObject.name + "]:[" + self.FsmName + "]:[StringVariables] - '" + fsmString.Name + "': '" + fsmString.Value + "'");
                            }

                            FsmVector2[] vector2Variables = fsmVariables.Vector2Variables;
                            foreach (FsmVector2 fsmVector in vector2Variables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[Vector2Variables] - '{fsmVector.Name}': '({fsmVector.Value.x}, {fsmVector.Value.y})'");
                            }

                            FsmVector3[] vector3Variables = fsmVariables.Vector3Variables;
                            foreach (FsmVector3 fsmVector2 in vector3Variables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[Vector3Variables] - '{fsmVector2.Name}': '({fsmVector2.Value.x}, {fsmVector2.Value.y}, {fsmVector2.Value.z})'");
                            }

                            FsmColor[] colorVariables = fsmVariables.ColorVariables;
                            foreach (FsmColor fsmColor in colorVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[ColorVariables] - '{fsmColor.Name}': '{fsmColor.Value}'");
                            }

                            FsmRect[] rectVariables = fsmVariables.RectVariables;
                            foreach (FsmRect fsmRect in rectVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[RectVariables] - '{fsmRect.Name}': '{fsmRect.Value}'");
                            }

                            FsmQuaternion[] quaternionVariables = fsmVariables.QuaternionVariables;
                            foreach (FsmQuaternion fsmQuaternion in quaternionVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[QuaternionVariables] - '{fsmQuaternion.Name}': '{fsmQuaternion.Value}'");
                            }

                            FsmGameObject[] gameObjectVariables = fsmVariables.GameObjectVariables;
                            foreach (FsmGameObject fsmGameObject in gameObjectVariables)
                            {
                                Logger.Log($"[{self.gameObject.name}]:[{self.FsmName}]:[GameObjectVariables] - '{fsmGameObject.Name}': '{fsmGameObject.Value}'");
                            }
                        }, num + 1);
                    }
                }
            }
        }

        public static void Log(this PlayMakerFSM self)
        {
            Log("FSM \"" + self.name + "\"");
            Log($"{self.FsmStates.Length} States");
            FsmState[] fsmStates = self.FsmStates;
            foreach (FsmState fsmState in fsmStates)
            {
                Log("\tState \"" + fsmState.Name + "\"");
                FsmTransition[] transitions = fsmState.Transitions;
                foreach (FsmTransition fsmTransition in transitions)
                {
                    Log("\t\t-> \"" + fsmTransition.ToState + "\" via \"" + fsmTransition.EventName + "\"");
                }
            }

            Log($"{self.FsmGlobalTransitions.Length} Global Transitions");
            FsmTransition[] fsmGlobalTransitions = self.FsmGlobalTransitions;
            foreach (FsmTransition fsmTransition2 in fsmGlobalTransitions)
            {
                Log("\tGlobal Transition \"" + fsmTransition2.EventName + "\" to \"" + fsmTransition2.ToState + "\"");
            }
        }

        private static void Log(string msg)
        {
            Logger.Log("[SFCore]:[Util]:[FsmUtil]:" + msg);
        }
    }
}
