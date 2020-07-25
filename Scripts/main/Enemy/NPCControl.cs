using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition = 0, // Use this transition to represent a non-existing transition in your system
    ChasingPlayer = 1,
    AutoPatrol = 2,
    AttackPlayer = 3,
    GuardPlayer = 4,
}

public enum StateID
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your system	
    ChasingID = 1,
    PatrolID = 2,
    AttackID = 3,
    GuardID = 4,
}

public class NPCControl : MonoBehaviour {

    public Transform player;
    public Transform[] path;
    private FSMSystem fsm;

    public void SetTransition(Transition t) { fsm.PerformTransition(t); }

    public void Start()
    {
        MakeFSM();
    }

    public void FixedUpdate()
    {
        if (GameManager.Instance.GameRunning)
        {
            fsm.CurrentState.Reason();
            fsm.CurrentState.Act();
        }
    }

    private void MakeFSM()
    {
        PatrolState  patrol = new PatrolState(player,transform,path);
        patrol.AddTransition(Transition.GuardPlayer, StateID.GuardID);

        GuardState guard = new GuardState(player, transform);
        guard.AddTransition(Transition.AutoPatrol, StateID.PatrolID);
        guard.AddTransition(Transition.ChasingPlayer, StateID.ChasingID);

        ChaseState chase = new ChaseState(player, transform);
        chase.AddTransition(Transition.GuardPlayer, StateID.GuardID);
        chase.AddTransition(Transition.AttackPlayer, StateID.AttackID);

        AttackState attack = new AttackState(player, transform);
        attack.AddTransition(Transition.GuardPlayer, StateID.GuardID);
        attack.AddTransition(Transition.ChasingPlayer, StateID.ChasingID);

        fsm = new FSMSystem();
        
        fsm.AddState(patrol);
        fsm.AddState(guard);
        fsm.AddState(chase);
        fsm.AddState(attack);
    }

}

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : FSMState
{
    private int currentWayPoint;
    private Transform[] waypoints;
    private Transform player;
    private Transform npc;
    private Rigidbody npcRgb;
    private NPCControl npcControl;

    public PatrolState(Transform player, Transform npc, Transform[] path)
    {
        this.player = player;
        this.npc = npc;
        npcRgb = npc.gameObject.GetComponent<Rigidbody>();
        npcControl = npc.gameObject.GetComponent<NPCControl>();
        currentWayPoint = 0;
        waypoints = path;
        stateID = StateID.PatrolID;
    }

    public override void Reason()
    {
        float distance = Vector3.Magnitude(player.position - npc.position);
        if (distance > 19f && distance < 24f)
        {
            npcControl.SetTransition(Transition.GuardPlayer);
        }
    }

    public override void Act()
    {
        Vector3 vel = npcRgb.velocity;
        Vector3 moveDir = waypoints[currentWayPoint].position - npc.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
        }
        else
        {
            vel = moveDir.normalized * 5;
            // Rotate towards the waypoint
            npc.transform.rotation = Quaternion.Slerp(npc.rotation, Quaternion.LookRotation(moveDir), 5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);//只旋转y轴

        }
        // Apply the Velocity
        npcRgb.velocity = vel;
    }

    public override void DoBeforeLeaving()
    {
        npcRgb.velocity = Vector3.zero;
    }



}

/// <summary>
/// 攻击状态
/// </summary>
public class AttackState : FSMState
{
    private Transform player;
    private Transform npc;
    private NPCControl npcControl;
    private Rigidbody npcRdb;

    public AttackState(Transform player, Transform npc)
    {
        this.player = player;
        this.npc = npc;
        npcControl = npc.gameObject.GetComponent<NPCControl>();
        npcRdb = npc.gameObject.GetComponent<Rigidbody>();
        stateID = StateID.AttackID;
    }

    public override void Reason()
    {
        float distance = Vector3.Magnitude(player.position - npc.position);
        if (distance > 19f)
        {
            npcControl.SetTransition(Transition.GuardPlayer);
        }
        else if (distance >= 16f && distance <= 19f)
        {
            npcControl.SetTransition(Transition.ChasingPlayer);
        }
    }

    public override void DoBeforeEntering()
    {
        npcRdb.velocity = Vector3.zero;
    }

    public override void Act()
    {
        Vector3 moveDir = player.transform.position - npc.transform.position;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(moveDir), 10 * Time.deltaTime);
        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
        npc.GetComponent<EnemyAttck>().Attack();
    }


}

/// <summary>
///警戒状态 调整敌方坦克的转向和位置 保持敌方坦克和玩家坦克之间的有效距离
///当敌方坦克和玩家坦克的距离在 20-25m 距离时进入该状态
/// </summary>
public class GuardState : FSMState
{
    private Transform player;
    private Transform npc;
    private NPCControl npcControl;
    private Rigidbody npcRdb;

    public GuardState(Transform player, Transform npc)
    {
        this.player = player;
        this.npc = npc;
        npcControl = npc.gameObject.GetComponent<NPCControl>();
        npcRdb = npc.gameObject.GetComponent<Rigidbody>();
        stateID = StateID.GuardID;
    }

    public override void Reason()
    {
        float distance = Vector3.Magnitude(player.position - npc.position);
        if (distance > 24f)
        {
            npcControl.SetTransition(Transition.AutoPatrol);
        }
        else if (distance <= 19f && distance >= 16f)
        {
            npcControl.SetTransition(Transition.ChasingPlayer);
        }
    }

    public override void DoBeforeEntering()
    {
        npcRdb.velocity = Vector3.zero;
    }

    public override void Act()
    {
        Vector3 moveDir = player.transform.position - npc.transform.position;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(moveDir), 10 * Time.deltaTime);
        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
  
    }

}


public class ChaseState : FSMState
{
    private Transform player;
    private Transform npc;
    private NPCControl npcControl;
    private Rigidbody npcRdb;

    public ChaseState(Transform player, Transform npc)
    {
        this.player = player;
        this.npc = npc;
        npcControl = npc.gameObject.GetComponent<NPCControl>();
        npcRdb = npc.gameObject.GetComponent<Rigidbody>();
        stateID = StateID.ChasingID;
    }

    public override void Reason()
    {
        float distance = Vector3.Magnitude(player.position - npc.position);
        if (distance < 16f)
        {
            npcControl.SetTransition(Transition.AttackPlayer);
        }
        else if (distance > 19f)
        {
            npcControl.SetTransition(Transition.GuardPlayer);
        }
    }

    public override void Act()
    {		
        Vector3 vel = npcRdb.velocity;
        Vector3 moveDir = player.position - npc.position;

        // Rotate towards the waypoint
        npc.rotation = Quaternion.Slerp(npc.rotation, Quaternion.LookRotation(moveDir), 10 * Time.deltaTime);
        npc.eulerAngles = new Vector3(0, npc.eulerAngles.y, 0);

        vel = moveDir.normalized * 3;

        // Apply the new Velocity
        npcRdb.velocity = vel;
    }

} // ChasePlayerState

public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    public void AddTransition(Transition trans, StateID id)
    {
        // Check if anyone of the args is invalid
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(trans, id);
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Check for NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    /// <summary>
    /// 返回所要转移状态的 StateID 
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    public virtual void DoBeforeEntering() { }

    public virtual void DoBeforeLeaving() { }

    public abstract void Reason();

    public abstract void Act();
}

public class FSMSystem
{
    private List<FSMState> states;
    // The only way one can change the state of the FSM is by performing a transition
    // Don't change the CurrentState directly
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    /// <summary>
    /// This method places new states inside the FSM,
    /// or prints an ERROR message if the state was already inside the List.
    /// First state added is also the initial state.
    /// </summary>
    public void AddState(FSMState s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() + " because state has already been added");
                return;
            }
        }
        states.Add(s);
    }

    /// <summary>
    /// This method delete a state from the FSM List if it exists, 
    ///   or prints an ERROR message if the state was not on the List.
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. If current state
    ///  doesn't have a target state for the transition passed, 
    /// an ERROR message is printed.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Check for NullTransition before changing the current state
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Check if the currentState has the transition passed as argument
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving();

                currentState = state;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }

    } // PerformTransition()

}
