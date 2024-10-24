using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlazeAISpace;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class BlazeAI : MonoBehaviour 
{
    [Header("GENERAL")]
    [Tooltip("Enabling this will make the agent use root motion, this gives more accurate and realistic movement but any move speed property will not be considered as the speed will be that of the animation.")]
    public bool useRootMotion = false;
    [Tooltip("This will be the center position of the AI used in many calculations. Best to position at the pelvis/torso area.")]
    public Vector3 centerPosition = new Vector3(0, 1.2f, 0);
    [Tooltip("Will show the center position as a red sphere in the scene view.")]
    public bool showCenterPosition = true;
    public LayerMask groundLayers;

    
    [Header("AUDIOS"), Tooltip("All audios for Blaze are added in a scriptable then the behaviours as well as Blaze read from this scriptable. To create an audio scriptable: Right-click in the Project window > Create > Blaze AI > Audio Scriptable.")]
    public AudioScriptable audioScriptable;
    public AudioSource agentAudio;


    [Header("PATROL ROUTES & TURNING ANIMS")]
    public BlazeAISpace.Waypoints waypoints;


    [Header("VISION & ADDING ENEMIES")]
    public BlazeAISpace.Vision vision;

    
    [Header("CHECK FOR ENEMY CONTACT"), Tooltip("Check if a hostile got too close and came in contact with the AI. If so, will turn to attack state.")]
    public bool checkEnemyContact;
    [Min(0), Tooltip("The radius for checking if a hostile came in contact.")]
    public float enemyContactRadius = 1.2f;
    [Tooltip("Shows the radius as a grey wire sphere in the scene view.")]
    public bool showEnemyContactRadius;


    [Header("FRIENDLY AI")]
    [Tooltip("If this is enabled the AI will never turn to attack state when seeing a hostile tag or on enemy contact until this property is set to false. TAKE NOTE: specifically calling the API SetEnemy(player) or Hit(player) will force disable friendly mode.")]
    public bool friendly;


    [Header("DISTANCE CULLING")]
    [Tooltip("Disable this gameobject when it exceeds the distance set in the BlazeAIDistanceCulling component. This will drastically improve performance.")]
    public bool distanceCull;


    [Header("UNREACHABLE ENEMIES")]
    [Tooltip("If enabled and an enemy is unreachable or becomes unreachable the AI will ignore it and continue patrolling. If turned off, the AI will not ignore the enemy and turn to attack state and wait for it to be reachable.")]
    public bool ignoreUnreachableEnemy;
    [Tooltip("If an unreachable enemy has been detected, the AI will choose ONE random point from this array to move to in alert state then continue it's patrol. If array length is 0 OR the chosen index == Vector3.zero then the AI will turn to alert state and patrol it's normal waypoints set in the Waypoints class.")]
    public Vector3[] fallBackPoints;
    [Tooltip("Will show the fallback points if unreachable in the scene view as cyan-colored spheres.")]
    public bool showPoints = false;

    
    [Header("WARN EMPTY BEHAVIOURS"), Tooltip("Will print in the console to warn you if any behaviour is empty.")]
    public bool warnEmptyBehavioursOnStart = true;

    
    [Header("NORMAL STATE")]
    public bool useNormalStateOnAwake;
    public MonoBehaviour normalStateBehaviour;

    
    [Header("ALERT STATE")]
    public bool useAlertStateOnAwake;
    public MonoBehaviour alertStateBehaviour;
    

    public MonoBehaviour attackStateBehaviour;
    public bool coverShooterMode;
    public MonoBehaviour coverShooterBehaviour;
    public MonoBehaviour goingToCoverBehaviour;

    
    [Header("SURPRISED STATE")]
    public bool useSurprisedState;
    public MonoBehaviour surprisedStateBehaviour;

    
    [Header("DISTRACTED STATE")]
    public bool canDistract = true;
    public MonoBehaviour distractedStateBehaviour;
    [Range(0, 100), Tooltip("If a distraction triggers a group of agents, the highest priority AI only is sent to the distraction point. Here you can set which AI is more prone to check the distraction.")]
    public float priorityLevel = 50;

    [Header("TURN ALERT"), Tooltip("If enabled and the AI gets distracted in normal state. It'll play the alert movement animation as well as have the alert vision and when the distracted state is finished, the AI will return to alert state instead of normal. Enabling this option makes the AI act exactly as if it's been distracted during alert state.")]
    public bool turnAlertOnDistract;
    
    [Header("AUDIOS"), Tooltip("Play audio when distracted. Set the audios in the audio scriptable in the General tab.")]
    public bool playDistractedAudios;

    
    [Header("HIT STATE")]
    public MonoBehaviour hitStateBehaviour;

    
    [Header("DEATH")]
    public string deathAnim;
    [Min(0)]
    public float deathAnimT = 0.25f;
    
    [Header("AUDIO")]
    [Space(7), Tooltip("Set your audios in the audio scriptable in the General Tab in Blaze AI.")]
    public bool playDeathAudio;
    
    [Header("CALL OTHERS")]
    [Space(7), Min(0), Tooltip("The radius of calling other AIs on death. The will appear in the scene view as a cyan colored wire sphere.")]
    public float deathCallRadius = 10f;
    [Tooltip("The layers of the AIs to call.")]
    public LayerMask agentLayersToDeathCall; 
    [Tooltip("If enabled, this will show the death call radius in the scene view as a cyan colored wire sphere.")]
    public bool showDeathCallRadius;

    [Space(15), Tooltip("Set an event to trigger on death.")]
    public UnityEngine.Events.UnityEvent deathEvent;

    [Header("DESTROY")]
    [Tooltip("Will destroy this gameobject on death.")]
    public bool destroyOnDeath;
    [Min(0), Tooltip("The time to pass in death before destroying the gameobject.")]
    public float timeBeforeDestroy = 5;


    [Header("COMPANION MODE")]
    [Tooltip("If enabled, the AI will trigger the companion behaviour to follow the target.")]
    public bool companionMode;
    [Tooltip("Set the player or any other target you want this AI to follow and be a companion to.")]
    public Transform companionTo;
    [Tooltip("The companion behaviour script.")]
    public MonoBehaviour companionBehaviour;

    
    #region SYSTEM VARIABLES

    Animator anim;
    public NavMeshAgent navmeshAgent { get; private set; }
    CapsuleCollider capsuleCollider;
    NavMeshPath path;
    [HideInInspector] public AnimationManager animManager;


    public enum State 
    {
        normal,
        alert,
        attack,
        goingToCover,
        sawAlertTag,
        returningToAlert,
        surprised,
        distracted,
        hit,
        death
    }


    public State state { get; private set; }
    public int waypointIndex { get; private set; }
    public bool isPathReachable { get; private set; }
    public Vector3 endDestination { get; private set; }
    public State previousState { get; private set; }
    public GameObject enemyToAttack { get; private set; }
    public Vector3 checkEnemyPosition { get; set; }
    public float captureEnemyTimeStamp { get; private set; }
    public Vector3 enemyColPoint { get; private set; }
    public bool isAttacking { get; set; }
    public float distanceToEnemySqrMag { get; private set; }
    public float distanceToEnemy { get; private set; }
    public Vector3 enemyPosOnSurprised { get; private set; }
    public string sawAlertTagName { get; private set; }
    public Vector3 sawAlertTagPos { get; private set; }
    public Transform sawAlertTagObject { get; private set; }
    public string defaultNormalIdleAnim { get; set; }
    public string defaultAlertIdleAnim { get; set; }
    
    
    // read by behaviours
    public bool movedToLocation { get; set; }
    public bool stayIdle { get; set; }
    public bool isIdle { get; set; }
    public bool hitRegistered { get; set; }
    public bool changedState { get; set; }
    public GameObject hitEnemy { get; set; }
    public bool hitWhileInCover { get; set; }
    public bool tookCover { get; set; }
    public bool callOthersOnHit { get; private set; }
    public bool stayAlertUntilPos { get; set; }

    
    public Vector3 pathCorner { get; set; }
    Vector3 lastCalculatedPath;
    Vector3 startPosition;
    Queue<Vector3> cornersQueue;


    bool useNormalStateOnAwakeInspectorState;
    bool useAlertStateOnAwakeInspectorState;
    bool isturningToCorner;
    MonoBehaviour lastEnabledBehaviour;


    int visionCheckElapsed = 0;
    int closestPointElapsed = 5;
    int checkSurroundingElapsed = 0;
    int enemyCaughtForFrames = 0;


    Collider[] visionColl = new Collider[20];
    Transform visionT;
    Collider ignoredEnemy = null;

    #endregion

    #region UNITY METHODS

    void Start()
    {
        anim = GetComponent<Animator>();
        animManager = new AnimationManager(anim);
        capsuleCollider = GetComponent<CapsuleCollider>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        
        path = new NavMeshPath();
        cornersQueue = new Queue<Vector3>();


        SetAgentAudio();
        
        
        startPosition = transform.position;
        waypointIndex = -1;


        ComponentsOnAwake();
        vision.CheckHostileAndAlertItemEqual();


        // set state on awake
        if (useNormalStateOnAwake) {
            SetState(State.normal);
            return;
        }

        
        SetState(State.alert);
    }

    void Update()
    {
        // set the vision to head if available
        if (vision.head == null) {
            visionT = transform;
        }
        else {
            visionT = vision.head;
        }

        
        // only apply the attack state vision if enemy caught for 3 or more frames
        // so count how many frames
        CountVisionCaughtEnemyFrames();

        
        // always apply the anim root speed if using root motion
        if (useRootMotion) {
            Vector3 worldDeltaPosition = navmeshAgent.nextPosition - transform.position;

            if (worldDeltaPosition.magnitude > navmeshAgent.radius) {
                navmeshAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
            }
        }


        // enable the state's behaviour
        switch (state) {
            case State.normal:
                if (companionMode) {
                    EnableBehaviour(companionBehaviour);
                    break;
                }

                EnableBehaviour(normalStateBehaviour);
                break;
            case State.alert:
                if (companionMode) {
                    EnableBehaviour(companionBehaviour);
                    break;
                }

                EnableBehaviour(alertStateBehaviour);
                break;
            case State.attack:
                if (coverShooterMode) {
                    EnableBehaviour(coverShooterBehaviour);
                    break;
                }
                
                EnableBehaviour(attackStateBehaviour);
                break;
            case State.sawAlertTag:
                EnableBehaviour(vision.alertTags[vision.GetAlertTagIndex(sawAlertTagName)].behaviourScript);
                break;
            case State.distracted:
                EnableBehaviour(distractedStateBehaviour);
                break;
            case State.surprised:
                EnableBehaviour(surprisedStateBehaviour);
                break;
            case State.goingToCover:
                EnableBehaviour(goingToCoverBehaviour);
                break;
            case State.hit:
                EnableBehaviour(hitStateBehaviour);
                break;
            case State.returningToAlert:
                if (coverShooterMode) {
                    EnableBehaviour(coverShooterBehaviour);
                    break;
                }
                
                EnableBehaviour(attackStateBehaviour);
                break;
        }


        VisionCheck();
        SurroundingsCheck();        
        RemoveMoveToLocation();
    }
    
    void OnAnimatorMove()
    {
        if (!useRootMotion) {
            return;
        }


        if (anim == null) {
            return;
        }

        
        Vector3 position = anim.rootPosition;
        position.y = navmeshAgent.nextPosition.y;
        transform.position = position;
    }

    void OnValidate()
    {
        // choose either UseNormalStateOnAwake or UseAlertStateOnAwake (can't be both)
        if (!useAlertStateOnAwake && !useNormalStateOnAwake) {
            useNormalStateOnAwake = true;
        }

        if (useAlertStateOnAwake && useNormalStateOnAwake) {
            useAlertStateOnAwake = !useAlertStateOnAwakeInspectorState;
            useNormalStateOnAwake = !useNormalStateOnAwakeInspectorState;
        }

        useNormalStateOnAwakeInspectorState = useNormalStateOnAwake;
        useAlertStateOnAwakeInspectorState = useAlertStateOnAwake;


        // validate waypoints system
        if (waypoints != null) {
            waypoints.WaypointsValidation(transform.position);
        }


        DisableAllBehaviours();


        if (vision != null) {
            vision.DisableAllAlertBehaviours();
            vision.CheckHostileAndAlertItemEqual(true);

            if (vision.head == null) {
                visionT = transform;
            }
            else {
                visionT = vision.head;
            }
        }

        SetAgentAudio();
    }

    void OnDrawGizmosSelected() 
    {
        waypoints.Draw(transform.position);
        vision.ShowVisionSpheres(visionT);
        ShowCenterPosition();
        ShowEnemyContactRadius();
        ShowDeathCallRadius();
        ShowFallBackPoints();
    }

    // enable & set important components on awake
    void ComponentsOnAwake()
    {
        navmeshAgent.enabled = true;
        navmeshAgent.stoppingDistance = 0;
        navmeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        NavMesh.avoidancePredictionTime = 0.5f;
        capsuleCollider.isTrigger = true;


        // if distance culling enabled then add this transform to the list
        if (distanceCull) {
            AddDistanceCulling();
        }


        if (warnEmptyBehavioursOnStart) {
            CheckEmptyBehaviours();
        }
    }
    
    // print in the console if a behaviour is missing a script
    void CheckEmptyBehaviours()
    {
        if (useNormalStateOnAwake) {
            if (normalStateBehaviour == null) {
                Debug.Log($"Normal State Behaviour is empty in game object: {gameObject.name}.");
            }
        }

        if (useAlertStateOnAwake) {
            if (alertStateBehaviour == null) {
                Debug.Log($"Alert State Behaviour is empty in game object: {gameObject.name}.");
            }
        }

        if (canDistract) {
            if (distractedStateBehaviour == null) {
                Debug.Log($"Distracted State Behaviour is empty in game object: {gameObject.name}.");
            }
        }

        if (useSurprisedState) {
            if (surprisedStateBehaviour == null) {
                Debug.Log($"Surprised State Behaviour is empty in game object: {gameObject.name}.");
            }
        }

        if (!coverShooterMode) {
            if (attackStateBehaviour == null) {
                Debug.Log($"Attack State Behaviour is empty in game object: {gameObject.name}.");
            }
        }
        else {
            if (coverShooterBehaviour == null) {
                Debug.Log($"Cover Shooter Behaviour is empty in game object: {gameObject.name}.");
            }

            if (goingToCoverBehaviour == null) {
                Debug.Log($"Going To Cover Behaviour is empty in game object: {gameObject.name}.");
            }
        }

        if (hitStateBehaviour == null) {
            Debug.Log($"Hit State Behaviour is empty in game object: {gameObject.name}.");
        }
    }

    void OnDisable() 
    {
        DisableAllBehaviours();
        enemyCaughtForFrames = 0;
    }

    void OnEnable()
    {
        // if blaze is enabled -> enable navmesh agent component
        if (navmeshAgent != null) {
            navmeshAgent.enabled = true;
        }

        SetAgentAudio();
    }

    #endregion

    #region MOVEMENT
    
    // move to location
    public bool MoveTo(Vector3 location, float moveSpeed, float turnSpeed, string animName=null, float animT=0.25f, string dir="front", float closestPointDistance=0) {
        if (dir == "front") {
            if ((!isAttacking || enemyToAttack == null) && (lastCalculatedPath == location) && cornersQueue.Count == 0) {
                // check if AI is already at the min possible distance from location
                float dist = (pathCorner - transform.position).sqrMagnitude;
                float minDis = navmeshAgent.radius * 2;
                
                if (dist <= (minDis * minDis)) {
                    return true;
                }
            }
        }
        

        // clear the corners
        cornersQueue.Clear();

        
        // calculates path corners and returns if reachable or not
        if (!IsPathReachable(location, true)) {
            if (dir != "front") {
                return false;
            }


            // if not set to check for closest point -> return false
            if (closestPointDistance <= 0) {
                return false;
            }

            
            closestPointElapsed++;


            // get closest point every 5 frames (for performance)
            if (closestPointElapsed > 5) {
                closestPointElapsed = 0;
                
                Vector3 point;
                if (ClosestNavMeshPoint(location, closestPointDistance, out point)) {
                    location = point;
                }
                else {
                    // vector zero means couldn't find a good point
                    if (point == Vector3.zero) {
                        return false;
                    }
                }
            }
        }
        
        
        // add the corners to queue so we can follow
        for (int i=0; i<path.corners.Length; i++) {
            cornersQueue.Enqueue(path.corners[i]);
        }
        

        // get the next corner
        GetNextCorner();

        
        return GoToCorner(animName, animT, moveSpeed, turnSpeed, dir);
    }

    // follow the path corners
    bool GoToCorner(string anim, float animT, float moveSpeed, float turnSpeed, string dir)
    {
        float currentDistance = (new Vector3(pathCorner.x, pathCorner.y, pathCorner.z) - transform.position).sqrMagnitude;
        float minDistance = 0f;

        bool isLastCorner = false;
        bool isReachedEnd = false;

        
        // check if going to last corner or not
        if (cornersQueue.Count > 0) {
            minDistance = 0.3f;
        }
        else {
            minDistance = navmeshAgent.radius * 2;
            isLastCorner = true;
        }

        
        // if reached min distance of corner
        if (currentDistance <= (minDistance * minDistance)) {
            if (isLastCorner) {
                isReachedEnd = true;
            }
            else {
                GetNextCorner();
            }
        }
        

        // turning to path corner shouldn't be done in attack states
        if (state != State.attack && state != State.goingToCover) {
            // turn to face path corner
            if (waypoints.useMovementTurning) {
                // check if should turning
                if (isturningToCorner) {
                    // if hadn't fully turned yet -> return
                    if (!TurnTo(pathCorner, GetTurnAnim("left"), GetTurnAnim("right"), waypoints.turningAnimT, waypoints.turnSpeed, waypoints.useTurnAnims)) {
                        return false;
                    }

                    // reaching this point means turning has completed
                    isturningToCorner = false;
                }

                
                // calculate the dot prod of the path corner
                float dotProd = Vector3.Dot((pathCorner - transform.position).normalized, transform.forward);
                
                
                // determine if should be turning
                if (dotProd < Mathf.Clamp(waypoints.movementTurningSensitivity, -1, 0.97f)) {
                    // if should turn then flag as so and return
                    isturningToCorner = true;
                    return false;
                }
            }
        }


        // rotate to corner
        RotateTo(pathCorner, turnSpeed);
        
        
        // play passed move animation
        animManager.Play(anim, animT);

        
        // only applied if not using root motion -> if using root motion, speed apply is in OnAnimatorMove()
        if (!useRootMotion) {
            Vector3 transformDir;

            switch (dir) {
                case "backwards":
                    transformDir = -transform.forward;
                    break;
                case "left":
                    transformDir = -transform.right;
                    break;
                case "right":
                    transformDir = transform.right;
                    break;
                default:
                    transformDir = transform.forward;
                    break;
            }

            navmeshAgent.Move(transformDir * Time.deltaTime * moveSpeed);
        }


        return isReachedEnd;
    }

    // get the next corner
    void GetNextCorner()
    {
        if (cornersQueue.Count > 0) {
            pathCorner = cornersQueue.Dequeue();
        }
    }

    // smooth rotate agent to location
    public void RotateTo(Vector3 location, float speed)
    {
        Quaternion lookRotation = Quaternion.LookRotation((location - transform.position).normalized);
        lookRotation = new Quaternion(0f, lookRotation.y, 0f, lookRotation.w);
        transform.rotation = Quaternion.Slerp(new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), lookRotation, speed * Time.deltaTime);
    }

    // set waypoint index to the next waypoint
    public Vector3 NextWayPoint()
    {   
        if (waypointIndex >= waypoints.waypoints.Count - 1) {
            if (waypoints.loop) waypointIndex = 0;
        }
        else {
            waypointIndex++;
        }

        endDestination = waypoints.waypoints[waypointIndex];
        return endDestination;
    }

    // returns true if there is a waypoint rotation
    public bool CheckWayPointRotation()
    {
        if ((waypoints.waypointsRotation[waypointIndex].x != 0 || waypoints.waypointsRotation[waypointIndex].y != 0)) {
            float dotProd = Vector3.Dot((new Vector3(transform.position.x + waypoints.waypointsRotation[waypointIndex].x, transform.position.y, transform.position.z + waypoints.waypointsRotation[waypointIndex].y) - transform.position).normalized, transform.forward);
            
            if (dotProd < 0.97f) {
                return true;
            }else{
                return false;
            }
        }
        else {
            return false;
        }
    }
    
    // turns AI to waypoint rotations and returns true when done
    public bool WayPointTurning()
    {   
        // set the turning anims of the state
        string leftTurnAnim = GetTurnAnim("left");
        string rightTurnAnim = GetTurnAnim("right");
        float animT = waypoints.turningAnimT;

        if ((waypoints.waypointsRotation[waypointIndex].x != 0 || waypoints.waypointsRotation[waypointIndex].y != 0)) {
            Vector3 wayPointDir = new Vector3(transform.position.x + waypoints.waypointsRotation[waypointIndex].x, transform.position.y, transform.position.z + waypoints.waypointsRotation[waypointIndex].y);
            return TurnTo(wayPointDir, leftTurnAnim, rightTurnAnim, animT);
        }
        else {
            return true;
        }
    }

    // turn to location and returns true when done
    public bool TurnTo(Vector3 location, string leftTurnAnim = null, string rightTurnAnim = null, float animT = 0.25f, float turnSpeed=0, bool playAnims = true)
    {
        location = new Vector3(location.x, transform.position.y, location.z);

        // get dir (left or right)
        int waypointTurnDir = AngleDir
        (transform.forward, 
        location - transform.position, 
        transform.up);

        
        float dotProd = Vector3.Dot((location - transform.position).normalized, transform.forward);

        if (dotProd >= 0.97f) {
            return true;
        }


        // turn right if dir is 1
        if (playAnims) {
            if (waypointTurnDir == 1) {
                animManager.Play(rightTurnAnim, animT);
            }
            else {
                animManager.Play(leftTurnAnim, animT);
            }
        }


        if (turnSpeed == 0) {
            turnSpeed = waypoints.turnSpeed;
        }
        

        RotateTo(location, turnSpeed);

       
        return false;
    }

    // return 1 if location is to the right and -1 if left
    int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) 
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        
        if (dir > 0f) {
            return 1;
        } else if (dir < 0f) {
            return -1;
        } else {
            return 0;
        }
    }

    // return the turn anim name depending on current state
    string GetTurnAnim(string dir) 
    {
        if (state == State.normal) {
            if (dir == "right") return waypoints.rightTurnAnimNormal;
            else return waypoints.leftTurnAnimNormal;
        }
        else{
            if (dir == "right") return waypoints.rightTurnAnimAlert;
            else return waypoints.leftTurnAnimAlert;
        }
    }

    #endregion

    #region DISTRACTED STATE
    
    // distract the AI
    public void Distract(Vector3 location, bool playAudio = true)
    {
        if (!canDistract || state == State.attack || !enabled || state == State.hit || state == State.death || companionMode) {
            return;
        }
        
        
        // get nearest navmesh position
        Vector3 pos = GetSamplePosition(ValidateYPoint(location), navmeshAgent.radius * 2);
        if (pos == Vector3.zero || pos == endDestination) {
            return;
        }

        
        endDestination = pos;


        if (turnAlertOnDistract) SetState(State.alert);


        if (state != State.distracted) {
            if (state == State.returningToAlert) previousState = State.alert;
            else previousState = state;
        }
        else {
            // if already in distracted state -> re-enable behaviour to reset
            distractedStateBehaviour.enabled = false;
            distractedStateBehaviour.enabled = true;
        }


        // sometimes this parameter is passed as false to avoid all distracted agents playing audio
        // which will sound distorted -> so only one agent in a group plays the audio
        if (playAudio) {
            if (playDistractedAudios) {
                // play audio only if not already in distracted state
                if (state != State.distracted) {
                    if (!IsAudioScriptableEmpty()) {
                        PlayAudio(audioScriptable.GetAudio(AudioScriptable.AudioType.Distracted));
                    }
                }
            }
        }


        // change the state to distracted
        SetState(State.distracted);
    }
    
    #endregion
    
    #region VISION

    // vision pass
    void VisionCheck()
    {
        // don't run vision if dead
        if (state == State.death) {
            return;
        }


        // run method once every pulse rate
        if (visionCheckElapsed < vision.pulseRate) {
            visionCheckElapsed++;
            return;
        }


        visionCheckElapsed = 0;
        List<Collider> enemiesToAttack = new List<Collider>();
        Vector3 npcDir = transform.position + centerPosition;


        // set the vision range and angle according to state
        float range, angle;
        
        switch (state) {
            case State.normal:
                angle = vision.visionDuringNormalState.coneAngle;
                range = vision.visionDuringNormalState.sightRange;
                break;
            case State.alert:
                angle = vision.visionDuringAlertState.coneAngle;
                range = vision.visionDuringAlertState.sightRange;
                break;
            case State.attack:
                if (enemyToAttack && enemyCaughtForFrames >= 3) {
                    angle = vision.visionDuringAttackState.coneAngle;
                    range = vision.visionDuringAttackState.sightRange;
                }
                else {
                    angle = vision.visionDuringAlertState.coneAngle;
                    range = vision.visionDuringAlertState.sightRange;
                }
                break;
            case State.goingToCover:
                angle = vision.visionDuringAttackState.coneAngle;
                range = vision.visionDuringAttackState.sightRange;
                break;
            case State.returningToAlert:
                angle = vision.visionDuringAlertState.coneAngle;
                range = vision.visionDuringAlertState.sightRange;
                break;
            case State.sawAlertTag:
                angle = vision.visionDuringAlertState.coneAngle;
                range = vision.visionDuringAlertState.sightRange;
                break;
            case State.surprised:
                angle = vision.visionDuringAttackState.coneAngle;
                range = vision.visionDuringAttackState.sightRange;
                break;
            default:
                angle = PreviousStateVAngle();
                range = PreviousStateVRange();
                break;
        }


        // get the hostiles and alerts
        int visionCollNum = Physics.OverlapSphereNonAlloc(transform.position, range, visionColl, vision.hostileAndAlertLayers);
        
        for (int i=0; i<visionCollNum; i++) {
            // if caught collider is a child of the same AI then skip
            if (visionColl[i].transform.IsChildOf(transform)) {
                continue;
            }


            // if companion mode is on -> eliminate the companion from targeting 
            if (companionMode && companionTo != null && visionColl[i].transform.IsChildOf(companionTo)) {
                continue;
            }


            // check for alert tag
            int alertTagIndex = vision.GetAlertTagIndex(visionColl[i].tag);

            if (alertTagIndex >= 0) {
                GameObject alertObj = visionColl[i].transform.gameObject;
                
                // check if within vision angle
                if (Vector3.Angle(visionT.forward, (alertObj.transform.position - npcDir)) < (angle * 0.5f)) {
                    // check height
                    float alertheight = alertObj.transform.position.y - (centerPosition.y + visionT.position.y + vision.sightLevel + vision.maxSightLevel);
                    
                    if (alertheight < 0f) {
                        SawAlertTag(alertObj, alertTagIndex);
                    }
                }
            }


            // THE STARTING CODE FOR HOSTILE
            if (friendly) return;


            if (enemiesToAttack.Count >= 5) break;
            

            // check for hostile tags
            if (System.Array.IndexOf(vision.hostileTags, visionColl[i].tag) < 0) {
                continue;
            }

            
            Collider hostile = visionColl[i];

            
            // check if not within vision angle
            if (Vector3.Angle(visionT.forward, (hostile.transform.position - npcDir)) > (angle * 0.5f)) {
                continue;
            }

            
            // check height
            float suspectHeight = hostile.transform.position.y - (centerPosition.y + visionT.position.y + vision.sightLevel + vision.maxSightLevel);
            if (suspectHeight > 0f)
            {
                continue;
            }


            Collider[] enemyToAttackColliders = hostile.transform.GetComponentsInChildren<Collider>();
            int colSize = enemyToAttackColliders.Length;


            // set the raycast layers for vision
            int layersToHit;

            if (state != State.attack && state != State.goingToCover) {
                layersToHit = vision.layersToDetect | vision.hostileAndAlertLayers;
            }
            else {
                if (coverShooterMode) layersToHit = vision.hostileAndAlertLayers;
                else layersToHit = vision.layersToDetect | vision.hostileAndAlertLayers;
            }


            // prevent adding colliders of the same gameobject
            bool exists = false;
            if (enemiesToAttack.Count > 0) {
                foreach (var coll in enemiesToAttack) {
                    if (coll.transform.IsChildOf(hostile.transform)) {
                        exists = true;
                        break;
                    }
                }
            }
            

            if (!exists) {
                // check the detection score
                if (colSize <= 2) {
                    if (RayCastObjectColliders(hostile.transform.gameObject, layersToHit, 1)) {
                        if (enemiesToAttack.Count < 5) {
                            enemiesToAttack.Add(hostile);
                        }
                    }
                }else{
                    // enemy is seen if more than half of it's colliders are seen
                    if (RayCastObjectColliders(hostile.transform.gameObject, layersToHit, colSize/2)) {
                        if (enemiesToAttack.Count < 5) {
                            enemiesToAttack.Add(hostile);
                        }
                    }
                }
            }
        }


        // if no valid enemies -> return
        if (enemiesToAttack.Count <= 0) {
            enemyToAttack = null;
            ignoredEnemy = null;

            return;
        }


        // if in hit state then don't continue until the state is changed
        if (state == State.hit) {
            return;
        }
        
        
        // sort the enemies by distance -> we always target the first one after sort (index 0)
        enemiesToAttack.Sort((x, y) => { return (transform.position - x.transform.position).sqrMagnitude.CompareTo((transform.position - y.transform.position).sqrMagnitude); });
        
        
        // if set to ignore unreachable enemy -> check if enemy is unreachable 
        if (ignoreUnreachableEnemy) {
            if (!IsPathReachable(enemiesToAttack[0].transform.position)) {
                // check if there's a previously ignored enemy
                if (ignoredEnemy != null) {
                    // if the previously ignored enemy didn't leave vision -> don't trigger function again until it gets out of vision and caught again
                    if (enemiesToAttack.Contains(ignoredEnemy)) {
                        return;
                    }

                    ignoredEnemy = null;
                }
                
                // if no previously ignored enemy -> trigger the function
                IgnoreEnemy(enemiesToAttack[0]);
                return;
            }
        }
        

        // target the least distance -> first item (index 0)
        enemyToAttack = enemiesToAttack[0].transform.gameObject;
        enemyColPoint = enemiesToAttack[0].ClosestPoint(enemiesToAttack[0].bounds.center);
        
        // reset check enemy position since AI has a target and no AI can call it 
        checkEnemyPosition = Vector3.zero;

        // make a timestamp
        captureEnemyTimeStamp = Time.time;


        // track distances
        distanceToEnemySqrMag = (new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z) - transform.position).sqrMagnitude;
        distanceToEnemy = Vector3.Distance(new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z), transform.position);


        // activate state
        if (state == State.normal) {
            Surprised();
        }
        else {
            if (state != State.distracted) {
                TurnToAttackState();
                return;
            }

            if (previousState != State.normal) {
                TurnToAttackState();
                return;
            }

            Surprised();
        }
    }

    // this method will trigger when the AI sees an alert tag
    void SawAlertTag(GameObject alertObj, int index)
    {
        // save the saw tag name and the object's position
        sawAlertTagName = alertObj.tag;
        sawAlertTagObject = alertObj.transform;

        Collider objectColl = sawAlertTagObject.GetComponent<Collider>();
        sawAlertTagPos = GetSamplePosition(ValidateYPoint(alertObj.transform.position), objectColl.bounds.size.x + objectColl.bounds.size.z);

        
        int layers = vision.layersToDetect | vision.hostileAndAlertLayers;
        
        // check if any collider is caught
        if (!RayCastObjectColliders(alertObj, layers, 1)) {
            return;
        }

        string fallBackTag;


        // check whether a fallback tag is set
        if (vision.alertTags[index].fallBackTag.Length <= 0) {
            fallBackTag = "Untagged";
        }
        else {
            fallBackTag = vision.alertTags[index].fallBackTag;
        }


        // if behaviour is empty -> tell the user
        if (vision.alertTags[index].behaviourScript == null) {
            Debug.Log($"Alert Tag: {sawAlertTagName} behaviour is empty so nothing will be enabled.");
        }


        // change the tag name of the alert object to the fallback tag
        alertObj.tag = fallBackTag;

        // set the state to saw alert tag and Update() enables the corresponding behaviour
        SetState(State.sawAlertTag);
    }
    
    // check if colliders of a gameobject are seen
    bool RayCastObjectColliders(GameObject go, int layersToHit, int minDetectionScore)
    {
        Collider[] objColls = go.transform.GetComponentsInChildren<Collider>();
        int colSize = objColls.Length;
        int detectionScore = 0;
        

        // check if raycast can hit target colliders
        for (int i=0; i<colSize; i++) {
            Collider item = objColls[i];
            RaycastHit rayHit;

            Vector3 npcDir = transform.position + centerPosition;
            Vector3 colDir = item.ClosestPoint(item.bounds.center) - npcDir;

            
            // start with center raycast, if caught nothing -> top left, if caught nothing -> top right
            if (Physics.Raycast(npcDir, colDir, out rayHit, Mathf.Infinity, layersToHit)) {
                if (item.transform.IsChildOf(rayHit.transform) || rayHit.transform.IsChildOf(item.transform)) {
                    detectionScore++;
                }
                else {
                    // checking top left
                    colDir = (item.ClosestPoint(item.bounds.max) - npcDir);

                    if (Physics.Raycast(npcDir, colDir, out rayHit, Mathf.Infinity, layersToHit)) {
                        if (item.transform.IsChildOf(rayHit.transform) || rayHit.transform.IsChildOf(item.transform)) {
                            detectionScore++;
                        }
                        else {
                            // checking top right
                            colDir = (item.ClosestPoint(new Vector3(item.bounds.center.x - item.bounds.extents.x, item.bounds.center.y + item.bounds.extents.y, item.bounds.center.z + item.bounds.extents.z)) - npcDir);
                            
                            if (Physics.Raycast(npcDir, colDir, out rayHit, Mathf.Infinity, layersToHit)) {
                                if (item.transform.IsChildOf(rayHit.transform) || rayHit.transform.IsChildOf(item.transform)) {
                                    detectionScore++;
                                }
                            }
                        }
                    }
                }
            }
        }

        
        // if detection score is bigger or equal to the minimum required -> return true
        if (detectionScore >= minDetectionScore) {
            return true;
        }

        return false;
    }

    // get the vision angle of the previous state
    float PreviousStateVAngle()
    {
        if (previousState == State.normal) {
            return vision.visionDuringNormalState.coneAngle;
        }

        if (previousState == State.alert) {
            return vision.visionDuringAlertState.coneAngle;
        }

        if (previousState == State.attack) {
            return vision.visionDuringAttackState.coneAngle;
        }

        return 0;
    }

    // get the vision range of the previous state
    float PreviousStateVRange()
    {
        if (previousState == State.normal) {
            return vision.visionDuringNormalState.sightRange;
        }

        if (previousState == State.alert) {
            return vision.visionDuringAlertState.sightRange;
        }

        if (previousState == State.attack) {
            return vision.visionDuringAttackState.sightRange;
        }

        return 0;
    }

    // check if an enemy got too close and turn to attack if so
    void SurroundingsCheck()
    {
        // return if any of these conditions are true
        if (!checkEnemyContact || friendly || enemyToAttack || state == State.hit || state == State.death) {
            return;
        }


        GameObject closeTarget = CheckSurroundingForTarget();


        // check if an enemy got too close
        if (closeTarget == null) {
            return;
        }


        // if companion mode is on -> eliminate the companion being targeted
        if (companionMode && companionTo != null && closeTarget.transform.IsChildOf(companionTo)) {
            return;
        }


        // if caught collider is a child of the same AI then skip
        if (closeTarget.transform.IsChildOf(transform)) {
            return;
        }


        // if set to ignore unreachable enemy -> check if enemy is unreachable 
        if (ignoreUnreachableEnemy) {
            if (!IsPathReachable(closeTarget.transform.position)) {
                // check if there's a previously ignored enemy
                if (ignoredEnemy != null) {
                    // if the previously ignored enemy didn't leave vision -> don't trigger function again until it gets out of vision and caught again
                    if (closeTarget.transform.IsChildOf(ignoredEnemy.transform)) {
                        return;
                    }

                    ignoredEnemy = null;
                }
                
                // if no previously ignored enemy -> trigger the function
                IgnoreEnemy(closeTarget.GetComponent<Collider>());
                return;
            }
        }
            
        
        if (state == State.distracted) {
            if (previousState == State.normal) {
                Surprised();
                return;
            }

            SetEnemy(closeTarget);
            return;
        }


        if (state != State.normal) {
            SetEnemy(closeTarget);
            return;
        }


        SetEnemy(closeTarget, false);
        Surprised();
    }

    // check for an enemy character specific radius
    GameObject CheckSurroundingForTarget()
    {
        if (state == State.attack || state == State.goingToCover || state == State.surprised) {
            return null;
        }

        
        checkSurroundingElapsed++;

        
        if (checkSurroundingElapsed < 5) {
            return null;
        }

        
        checkSurroundingElapsed = 0;

        
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position + centerPosition, enemyContactRadius, hitColliders, vision.hostileAndAlertLayers);

        
        for (int i=0; i<numColliders; i++) {
            if (System.Array.IndexOf(vision.hostileTags, hitColliders[i].transform.tag) >= 0) {
                enemyPosOnSurprised = hitColliders[i].transform.position;
                
                if (RayCastObjectColliders(hitColliders[i].transform.gameObject, vision.layersToDetect | vision.hostileAndAlertLayers, 1)) {
                    return hitColliders[i].transform.gameObject;
                }
            }
        }


        return null;
    }

    // ignore passed enemy
    void IgnoreEnemy(Collider enemyColl)
    {
        enemyToAttack = null;
        isAttacking = false;


        // smooth transition to alert if in normal state
        if (state == State.normal) {
            ChangeState("alert");
        }
        else {
            // don't force out of these states
            if (state == State.hit || state == State.surprised || state == State.death) {
                return;
            }

            if (state != State.alert) {
                SetState(State.alert);
            }
        }


        ignoredEnemy = enemyColl;


        // if no fallback points set -> exit
        if (fallBackPoints.Length == 0) {
            return;
        }


        // choose a random point from the fallback points
        Vector3 chosenPoint = fallBackPoints[Random.Range(0, fallBackPoints.Length)];
        

        // if the randomly chosen point is zero -> exit
        if (chosenPoint == Vector3.zero) {
            return;
        }


        // if point != Vector3.zero then go to that point
        MoveToLocation(chosenPoint);
    }

    // count how many frames the enemy has been caught for
    // if 3 or more frames then apply the attack state vision
    void CountVisionCaughtEnemyFrames()
    {
        if (enemyToAttack) {
            if (enemyCaughtForFrames < 3) {
                enemyCaughtForFrames++;
            }
        }
        else {
            enemyCaughtForFrames = 0;
        }
    }

    #endregion

    #region ATTACK STATE
    
    // trigger the surprised state
    void Surprised()
    {
        if (state == State.hit) {
            return;
        }


        if (!useSurprisedState) {
            TurnToAttackState();
            return;
        }


        if (enemyToAttack) {
            enemyPosOnSurprised = enemyToAttack.transform.position;
        }


        SetState(State.surprised);
    }

    // turn to attack state
    public void TurnToAttackState()
    {
        if (state == State.attack || state == State.goingToCover || state == State.surprised || state == State.hit || state == State.death) {
            return;
        }
        
        SetState(State.attack);
    }

    // set an enemy and turn to attack state
    public void SetEnemy(GameObject enemy, bool turnStateToAttack = true, bool randomizePoint = false) 
    {
        if (state == State.death || !enabled) {
            return;
        }


        // force the friendly mode off if enemy is passed
        if (enemy != null) friendly = false;


        if (enemyToAttack && enemy) {
            if (!enemyToAttack.transform.IsChildOf(enemy.transform)) {
                return;
            }
        }
        

        // the randomized point is the point told to other AIs when calling them
        // so they don't climb on each another on arrival
        if (randomizePoint) {
            checkEnemyPosition = RandomSpherePoint(enemy.transform.position);
        }
        else {
            checkEnemyPosition = enemy.transform.position;
        }


        // check and set path of enemy
        if (!IsPathReachable(checkEnemyPosition)) {
            Vector3 point;

            if (ClosestNavMeshPoint(enemy.transform.position, Vector3.Distance(enemy.transform.position, transform.position), out point)) {
                checkEnemyPosition = point;
            }
            else {
                ChangeState("alert");
                return;
            }   
        }


        enemyColPoint = enemy.transform.position;
        enemyToAttack = enemy;
        

        if (turnStateToAttack) {
            SetState(State.attack);
        }
    }

    bool IsCompanion(GameObject enemy)
    {
        if (enemy == null) {
            return false;
        }

        // if companion mode is on -> eliminate the companion from targeting 
        if (companionMode && companionTo != null && enemy.transform.IsChildOf(companionTo)) {
            return true;
        }

        return false;
    }
    
    #endregion

    #region BEHAVIOURS & STATE MANAGEMENT
    
    // enable behaviour script of current state and disable others to maintain performance
    void EnableBehaviour(MonoBehaviour passedBehaviour)
    {
        if (passedBehaviour == null || passedBehaviour == lastEnabledBehaviour) return;


        // useful if behaviour script changed programmatically then disable that previous one
        if (lastEnabledBehaviour != null) lastEnabledBehaviour.enabled = false;

    
        MonoBehaviour[] behaviours = {normalStateBehaviour, 
        alertStateBehaviour, 
        attackStateBehaviour, 
        coverShooterBehaviour, 
        goingToCoverBehaviour, 
        distractedStateBehaviour, 
        surprisedStateBehaviour, 
        hitStateBehaviour,
        companionBehaviour};
        

        vision.DisableAllAlertBehaviours();


        int max = behaviours.Length;
        
        for (int i=0; i<max; i++) {
            if (behaviours[i] != null) {
                if (passedBehaviour == behaviours[i]) {
                    behaviours[i].enabled = true;
                    continue;
                } 
                
                behaviours[i].enabled = false;
            }
        }


        // enable saw alert tag behaviour 
        if (state == State.sawAlertTag) {
            passedBehaviour.enabled = true;
        }


        lastEnabledBehaviour = passedBehaviour;
    }

    // set the state of the AI to passed value
    public void SetState(State stateToTurnTo)
    {
        if (!System.Enum.IsDefined(typeof(State), stateToTurnTo)) {
            Debug.Log("Trying to set state to a value that is not defined.");
            return;
        }

        state = stateToTurnTo;

        CheckDistanceCullingWithState();
    }

    // disable all behaviours
    public void DisableAllBehaviours()
    {
        MonoBehaviour[] behaviours = {normalStateBehaviour, alertStateBehaviour, attackStateBehaviour, coverShooterBehaviour, goingToCoverBehaviour, distractedStateBehaviour, surprisedStateBehaviour, hitStateBehaviour, companionBehaviour};
        int max = behaviours.Length;
        
        for (int i=0; i<max; i++) {
            if (behaviours[i] != null) {
                behaviours[i].enabled = false;
            }
        }

        lastEnabledBehaviour = null;
    }

    void RemoveMoveToLocation()
    {
        if (state != State.alert && state != State.normal) {
            IgnoreMoveToLocation();
        }
    }
    
    #endregion

    #region CHARACTER
    
    // shows the center position as a red sphere in scene view
    void ShowCenterPosition()
    {
        if (!showCenterPosition) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + centerPosition, 0.1f);
    }

    // shows the enemy contact radius in scene view
    void ShowEnemyContactRadius()
    {
        if (!showEnemyContactRadius || !checkEnemyContact) {
            return;
        }

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position + centerPosition, enemyContactRadius);
    }

    // show fallback points if target unreachable
    void ShowFallBackPoints()
    {
        if (!ignoreUnreachableEnemy) {
            return;
        }


        if (!showPoints) {
            return;
        }

        
        // draw the wire spheres
        for (int i=0; i < fallBackPoints.Length; i++) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(fallBackPoints[i], 0.3f);
        }
    }
    
    #endregion

    #region AUDIOS

    void SetAgentAudio()
    {
        if (agentAudio) {
            return;
        }

        agentAudio = GetComponent<AudioSource>();
        
        if (agentAudio == null) {
            agentAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }

        agentAudio.playOnAwake = false;
    }
    
    public bool IsAudioScriptableEmpty()
    {
        if (audioScriptable == null) {
            Debug.Log("A behaviour checked for an audio scriptable to play an audio but the property was empty.");
            return true;
        }

        return false;
    }

    // play a passed audio
    public bool PlayAudio(AudioClip audio) 
    {
        // if passed audio is null -> return
        if (audio == null) {
            return false;
        }


        // same audio is playing -> return
        if (audio == agentAudio.clip && agentAudio.isPlaying) {
            return true;
        }


        agentAudio.Stop();
        agentAudio.clip = audio;
        agentAudio.Play();


        return true;
    }

    public void StopAudio()
    {
        agentAudio.Stop();
    }

    #endregion
    
    #region NAVMESH

    // get random point from navmesh
    public Vector3 RandomNavMeshLocation() 
    {
        Vector3 randomDirection = Random.insideUnitSphere * waypoints.randomizeRadius;
        randomDirection += startPosition;
        
        NavMeshHit hit;
        Vector3 point;


        NavMesh.SamplePosition(randomDirection, out hit, waypoints.randomizeRadius, 1);
        point = hit.position;


        float distance = (new Vector3(point.x, transform.position.y, point.y) - transform.position).sqrMagnitude;
        float radius = navmeshAgent.radius * 2;


        if (distance <= radius * radius) {
            RandomNavMeshLocation();
        }

        endDestination = point;
        return point;
    }

    // check whether point is on navmesh or not
    public bool IsPointOnNavMesh(Vector3 point, float radius = 2f)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, radius, NavMesh.AllAreas)) return true;
        else return false;
    }

    // get nearest position within point
    public Vector3 GetSamplePosition(Vector3 point, float range)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, range, NavMesh.AllAreas)) return hit.position;
        else return Vector3.zero;
    }

    // get the correct y position of an enemy
    public Vector3 ValidateYPoint(Vector3 pos)
    {
        if (!IsPointOnNavMesh(pos, 0.3f)) {
            RaycastHit downHit;
            
            if (Physics.Raycast(pos, -Vector3.up, out downHit, Mathf.Infinity, groundLayers)) {
                return downHit.point;
            }
        }

        return pos;
    }

    // is path status complete
    public bool IsPathReachable(Vector3 position, bool addAsLastCalcPath=false) 
    {
        // prevent calculating infinity
        if (position.x == Mathf.Infinity || position.z == Mathf.Infinity || position.y == Mathf.Infinity) {
            return false;
        }


        // calculate path
        bool pathValidation = NavMesh.CalculatePath(ValidateYPoint(transform.position), ValidateYPoint(position), NavMesh.AllAreas, path);
        
        
        // used in movement
        if (addAsLastCalcPath) {
            lastCalculatedPath = position;
        }


        // check calculation status
        if (path.status == NavMeshPathStatus.PathComplete) {
            isPathReachable = true;
        }
        else {
            isPathReachable = false;
        }
        
        
        // return path status
        return isPathReachable;
    }

    // get closest point to navmesh
    bool ClosestNavMeshPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < range; i++) {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(center, out hit, range, NavMesh.AllAreas)) {
                if (IsPathReachable(hit.position)) {
                    result = hit.position;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }

    // get a randomized point within a sphere location
    public Vector3 RandomSpherePoint(Vector3 point, float range = -1)
    {
        if (range <= -1) {
            range = navmeshAgent.height * 2;
        }
        
        return GetSamplePosition(ValidateYPoint(point + Random.onUnitSphere * range), 0.5f);
    }

    #endregion
    
    #region DEATH

    // destroy this AI gameobject
    void DestroyMe()
    {
        Destroy(gameObject);
    }


    // cancel the destroy call
    public void CancelDestroy()
    {
        CancelInvoke("DestroyMe");
    }

    
    // show the death call radius in scene view
    void ShowDeathCallRadius()
    {
        if (!showDeathCallRadius) {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + centerPosition, deathCallRadius);
    }

    #endregion
    
    #region DISTANCE CULLING
    
    // distance culling works only on normal and alert states -> if the AI is in any other state the culling is removed temporarily
    // so the AI can perform the state action -> until the AI goes back to either normal or alert
    void CheckDistanceCullingWithState()
    {
        if (!distanceCull) return;


        if (state != State.normal && state != State.alert) {
            RemoveDistanceCulling();
            return;
        }


        if (distanceCull) AddDistanceCulling();
    }
    
    #endregion

    #region INSPECTOR GUI

    // set the default normal, alert, attack and cover shooter behaviours
    public void SetPrimeBehaviours()
    {
        // setting normal behaviour
        NormalStateBehaviour normalBehaviour = GetComponent<NormalStateBehaviour>();

        if (normalBehaviour == null) {
            normalBehaviour = gameObject.AddComponent(typeof(NormalStateBehaviour)) as NormalStateBehaviour;
        }

        if (normalBehaviour != null) normalStateBehaviour = normalBehaviour;


        // setting alert behaviour
        AlertStateBehaviour alertBehaviour = GetComponent<AlertStateBehaviour>();

        if (alertBehaviour == null) {
            alertBehaviour = gameObject.AddComponent(typeof(AlertStateBehaviour)) as AlertStateBehaviour;
        }

        if (alertBehaviour != null) alertStateBehaviour = alertBehaviour;


        // setting attack behaviour
        AttackStateBehaviour attackBehaviour = GetComponent<AttackStateBehaviour>();

        if (attackBehaviour == null) {
            attackBehaviour = gameObject.AddComponent(typeof(AttackStateBehaviour)) as AttackStateBehaviour;
        }

        if (attackBehaviour != null) attackStateBehaviour = attackBehaviour;
        

        // setting cover shooter behaviour
        CoverShooterBehaviour shooterBehaviour = GetComponent<CoverShooterBehaviour>();

        if (shooterBehaviour == null) {
            shooterBehaviour = gameObject.AddComponent(typeof(CoverShooterBehaviour)) as CoverShooterBehaviour;
        }

        if (shooterBehaviour != null) coverShooterBehaviour = shooterBehaviour;


        // setting going to cover behaviour
        GoingToCoverBehaviour goingBehaviour = GetComponent<GoingToCoverBehaviour>();

        if (goingBehaviour == null) {
            goingBehaviour = gameObject.AddComponent(typeof(GoingToCoverBehaviour)) as GoingToCoverBehaviour;
        }

        if (goingBehaviour != null) goingToCoverBehaviour = goingBehaviour;


        DisableAllBehaviours();
    }


    // set the default surprised behaviour
    public void SetSurprisedBehaviour()
    {
        SurprisedStateBehaviour surprisedBehaviour = GetComponent<SurprisedStateBehaviour>();

        if (surprisedBehaviour == null) {
            surprisedBehaviour = gameObject.AddComponent(typeof(SurprisedStateBehaviour)) as SurprisedStateBehaviour;
        }

        if (surprisedBehaviour != null) surprisedStateBehaviour = surprisedBehaviour;

        DisableAllBehaviours();
    } 


    // set the default distracted behaviour
    public void SetDistractedBehaviour()
    {
        DistractedStateBehaviour distractedBehaviour = GetComponent<DistractedStateBehaviour>();

        if (distractedBehaviour == null) {
            distractedBehaviour = gameObject.AddComponent(typeof(DistractedStateBehaviour)) as DistractedStateBehaviour;
        }

        if (distractedBehaviour != null) distractedStateBehaviour = distractedBehaviour;

        DisableAllBehaviours();
    }


    // set the default hit behaviour
    public void SetHitBehaviour()
    {
        HitStateBehaviour hitBehaviour = GetComponent<HitStateBehaviour>();

        if (hitBehaviour == null) {
            hitBehaviour = gameObject.AddComponent(typeof(HitStateBehaviour)) as HitStateBehaviour;
        }

        if (hitBehaviour != null) hitStateBehaviour = hitBehaviour;

        DisableAllBehaviours();
    }


    public void SetCompanionBehaviour()
    {
        CompanionBehaviour cb = GetComponent<CompanionBehaviour>();

        if (cb == null) {
            cb = gameObject.AddComponent(typeof(CompanionBehaviour)) as CompanionBehaviour;
        }

        if (cb != null) companionBehaviour = cb;


        DisableAllBehaviours();
        companionMode = true;
        waypoints.useMovementTurning = false;
    }

    #endregion

    #region PUBLIC METHODS (APIS)
    
    // force the AI to move to a specified location
    public void MoveToLocation(Vector3 location, bool randomize=false)
    {
        if (state != State.normal && state != State.alert) {
            Debug.Log("MoveToLocation() only works when the AI is in normal and alert states.");
            return;
        }


        stayAlertUntilPos = true;


        // if randomize is set, get a random point within location sphere
        // to avoid sending all the AIs to the exact same point (this is good with groups)
        if (randomize) {
            endDestination = RandomSpherePoint(location);
        }
        else {
            endDestination = ValidateYPoint(location);
        }
        

        // end destination and this are both read by the normal and alert behaviours
        movedToLocation = true;
    }

    // ignore the forcing of movement to a certain location
    public void IgnoreMoveToLocation()
    {
        movedToLocation = false;
        stayAlertUntilPos = false;
    }

    // force the AI to go idle
    public void StayIdle()
    {
        if (state != State.normal && state != State.alert && state != State.distracted) {
            Debug.Log("StayIdle() only works when the AI is in normal, alert and distracted states.");
            return;
        }

        // this public property will be read by the behaviours
        stayIdle = true;
    }

    // check whether the AI is idle or not
    public bool IsIdle()
    {
        if (state != State.normal && state != State.alert && state != State.distracted) {
            Debug.Log("IsIdle() only works when the AI is in normal, alert and distracted states. Will return false.");
            return false;
        }

        // set by the behaviours
        return isIdle;
    }

    // force to attack target
    public void Attack()
    {
        if (state == State.death || !enabled) {
            Debug.Log("Attack() can't be called when the AI is in death state or Blaze AI disabled.");
            return;
        }   

        if (enemyToAttack == null) {
            Debug.Log("Attack() can't be called when the AI doesn't have a target.");
            return;
        }


        isAttacking = true;
        

        if (state != State.hit) {
            SetState(State.attack);
        }
    }

    // cancel current attack
    public void StopAttack()
    {
        isAttacking = false;
    }
    
    // change between normal and alert states only
    public void ChangeState(string state)
    {
        enabled = true;

        // just in case if user is respawning the AI from death
        navmeshAgent.enabled = true;

        CancelDestroy();

        // read by alert and normal state behaviours -> to avoid setting new destination
        changedState = true;


        if (state == "normal") {
            SetState(State.normal);
        }

        if (state == "alert") {
            SetState(State.alert);
        }
    }

    // set target 
    public void SetTarget(GameObject enemy, bool randomizePoint = false, bool applyAttackVisionForFrame = false) 
    {
        if (!enabled || state == State.death) {
            Debug.Log("Can't call SetTarget() when AI is in death state or Blaze AI is disabled. You have to call ChangeState(string state) first to revive the AI.");
            return;
        }

        
        if (enemy == null) {
            Debug.Log("There is no passed enemy.");
            return;
        }


        if (IsCompanion(enemy)) {
            Debug.Log("You can't SetTarget() on the companion. Companion Mode needs to be turned off first.");
            return;
        }


        if (enemyToAttack && enemy) {
            if (!enemyToAttack.transform.IsChildOf(enemy.transform)) {
                Debug.Log("Can't call SetTarget() when there's already a target chosen by the AI.");
                return;
            }
        }


        if (applyAttackVisionForFrame) enemyToAttack = enemy;
        
        SetEnemy(enemy, true, randomizePoint);
    }

    // hit the AI
    public void Hit(GameObject enemy = null, bool callOthers = false) 
    {
        if (state == State.death || !enabled) {
            Debug.Log("Hit() can't be called when the AI is in death state or Blaze AI is disabled.");
            return;
        }

        
        if (IsCompanion(enemy)) {
            enemy = null;
            Debug.Log("Hit() called on companion. It has been negated. This is just a warning.");
        }


        // read by the hit state behaviour
        hitEnemy = enemy;
        hitRegistered = true;


        // if AI has took cover and got hit -> flag this occurance to have the AI change cover
        if (state == State.goingToCover && tookCover) {
            hitWhileInCover = true;
        }


        // check in the hit behaviour if should call other AIs
        callOthersOnHit = callOthers;


        // change the state to hit
        SetState(State.hit);
    }

    // kill the AI
    public void Death(bool callOthers = false, GameObject enemy = null)
    {
        // return if already dead or Blaze disabled
        if (state == State.death || !enabled) {
            Debug.Log("Death() can't be called when the AI is in death state or Blaze AI is disabled.");
            return;
        }


        // set the state to death
        SetState(State.death);


        lastEnabledBehaviour = null;
        enemyToAttack = null;
        navmeshAgent.enabled = false;
        
        
        // disable all behaviours
        DisableAllBehaviours();
        vision.DisableAllAlertBehaviours();
        enemyCaughtForFrames = 0;


        // call others if set
        if (callOthers) {
            Collider[] agentsColl = new Collider[4];
            int agentsCollNum = Physics.OverlapSphereNonAlloc(transform.position, deathCallRadius, agentsColl, agentLayersToDeathCall);
        
            for (int i=0; i<agentsCollNum; i++) {
                BlazeAI script = agentsColl[i].GetComponent<BlazeAI>();

                // if caught collider is that of the same AI -> skip
                if (agentsColl[i].transform.IsChildOf(transform)) {
                    continue;
                }
                

                // if script doesn't exist -> skip
                if (script == null) {
                    continue;
                }
                

                // reaching this point means item is valid
                if (enemy) {
                    script.SetEnemy(enemy, true, true);
                }
                else {
                    script.SetEnemy(gameObject, true, true);
                }
            }
        }


        // invoke event and play death animation
        deathEvent.Invoke();
        if (gameObject.activeSelf) animManager.Play(deathAnim, deathAnimT);


        // play audio
        if (!IsAudioScriptableEmpty() && playDeathAudio) {
            PlayAudio(audioScriptable.GetAudio(AudioScriptable.AudioType.Death));
        }


        // if set to destroy game object
        if (destroyOnDeath) {
            RemoveDistanceCulling();
            Invoke("DestroyMe", timeBeforeDestroy);
            return;
        }
        

        // disable Blaze
        enabled = false;
    }

    // add agent to the distance culling list
    public void AddDistanceCulling()
    {
        if (BlazeAIDistanceCulling.instance) {
            BlazeAIDistanceCulling.instance.AddAgent(this);
            return;
        }

        Debug.LogWarning("Can't add agent to distance culling list since no distance culling instance can be found.");
    }

    // remove agent from the distance culling list
    public void RemoveDistanceCulling(bool enableObject=false)
    {
        if (BlazeAIDistanceCulling.instance) {
            BlazeAIDistanceCulling.instance.RemoveAgent(this);
            
            if (enableObject) {
                gameObject.SetActive(true);
            }

            return;
        }

        Debug.LogWarning("Can't remove agent from distance culling list since no distance culling instance can be found.");
    }

    // check if agent is in the distance culling list
    public bool CheckDistanceCulling()
    {
        if (BlazeAIDistanceCulling.instance) {
            return BlazeAIDistanceCulling.instance.CheckAgent(this);
        }

        return false;
    }
    
    #endregion
}