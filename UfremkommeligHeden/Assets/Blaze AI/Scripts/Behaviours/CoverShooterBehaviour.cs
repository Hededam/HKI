using UnityEngine;
using UnityEngine.Events;

namespace BlazeAISpace
{
    public class CoverShooterBehaviour : MonoBehaviour
    {
        [Min(0), Tooltip("Won't be considered if root motion is used.")] 
        public float moveSpeed = 5f;
        [Min(0)] public float turnSpeed = 5f;
        public string idleAnim;
        public string moveAnim;
        [Min(0)] public float idleMoveT = 0.25f;


        [Min(0), Tooltip("The safe distance between the AI and the target.")]
        public float distanceFromEnemy = 10f;
        [Min(0), Tooltip("The distance between AI and target when actually attacking. For example: if this is a ranged AI you want this value to be far since ranged enemies attack from a distance but if melee then this should be close.")]
        public float attackDistance = 1f;
        [Tooltip("Will check if any of these layers exist before attacking and if so will refrain from attacking until none of these layers exist. You can use this to avoid friendly fire with other AI agents.")]
        public LayerMask layersCheckOnAttacking = Physics.AllLayers;
        [Tooltip("Set the animation name of the attack.")]
        public string shootingAnim;
        public float shootingAnimT = 0.1f;
        [Tooltip("The event to trigger during a shot. This should be your shoot method.")]
        public UnityEvent shootEvent;
        [Min(0), Tooltip("The AI will shoot every cycle in a randomized time (seconds) between the two inputs. For a constant value set the two inputs to the same value.")]
        public Vector2 shootEvery = new Vector2(3, 5);
        [Min(0), Tooltip("The duration of the single shot.")] 
        public float singleShotDuration;
        [Min(0), Tooltip("The single shot event is triggered several times. Here you can set the time to pass between each shot until the total time of attack is finished.")]
        public float delayBetweenEachShot = 0.2f;
        [Min(0), Tooltip("The overall total time of shooting.")]
        public Vector2 totalShootTime = new Vector2(1, 3);


        [Tooltip("The chance of attacking the enemy on first sight. If set to [Always Attack] the agent will always attack first. If set to [Take Cover] then the agent will always go to cover first. If set to [Randomize] there's a 50/50 chance either going to cover or attacking.")]
        public FirstSightDecision firstSightDecision = FirstSightDecision.Randomize;
        [Tooltip("Do you want the agent to attack if it's cover is blown and an enemy can see it? If set to [Always Attack], the agent will leave it's cover and attack. If set to [Take Cover] it will refrain from attacking and find another cover. If no cover found it'll attack. If set to [Randomize] there's a 50/50 chance for either taking cover or attacking.")]
        public CoverBlownDecision coverBlownDecision = CoverBlownDecision.Randomize;
        [Tooltip("Should the AI open fire on the cover obstacle the enemy is hiding behind or only attack at the actual enemy. This property will only be taken to account if there's an actual cover the enemy is hiding behind.")]
        public AttackEnemyCover attackEnemyCover = AttackEnemyCover.Randomize;


        [Range(0, 10), Tooltip("How brave do you want the AI? The higher the number, the lower the CHANCE for the AI to go to cover after shooting is done. If the AI is brave enough and finished shooting, it'll wait for it's next attack in the open without going to cover. If set to 0 the AI will always return to cover after every shooting cycle. If set to 10, it'll never go to cover.")]
        public int braveMeter = 0;


        [Tooltip("The AIs have the ability to call other AIs for help when they see the target. Enabling this will call other agents to the location. If disabled, no AIs will be called.")]
        public bool callOthers;
        [Tooltip("The radius of the call.")]
        [Min(0)] public float callRadius;
        [Tooltip("Show the call radius as a white sphere in the scene view.")]
        public bool showCallRadius;
        [Tooltip("Select the layers of the Blaze AI agents you want to call.")]
        public LayerMask agentLayersToCall;
        [Tooltip("The call runs once every certain time. Here you can specifiy the amount of time (seconds) to pass everytime before calling other agents.")]
        public float callOthersTime;
        [Tooltip("If enabled, this AI will be called by other agents if they are in attack state. If disabled, this AI won't be called.")]
        public bool receiveCallFromOthers;


        [Tooltip("If enabled, the AI will back away if the target moves in too close.")]
        public bool moveBackwards;
        [Min(0), Tooltip("If the distance between the target and the AI is less than this the AI will back away.")]
        public float moveBackwardsDistance = 5f;
        [Min(0)]
        public float moveBackwardsSpeed = 3f;
        public string moveBackwardsAnim;
        public float moveBackwardsAnimT = 0.25f;
        [Tooltip("Allow to move backwards while attacking.")]
        public bool moveBackwardsAttack;


        [Tooltip("Will enable the AI to strafe in either direction when waiting to attack target.")]
        public bool strafe;
        public float strafeSpeed = 5f;
        [Min(-1), Tooltip("The amount of time to pass strafing before waiting and strafing again. A value will be randomized between the two inputs. For a constant value, set both inputs to the same value. For infinity and never stop strafing set both values to -1.")]
        public Vector2 strafeTime = new Vector2(3, 5);
        [Min(0), Tooltip("The amount of time to wait before strafing again.")]
        public Vector2 strafeWaitTime = new Vector2(0.2f, 1);
        [Tooltip("Left strafe animation name.")]
        public string leftStrafeAnim;
        [Tooltip("Right strafe animation name.")]
        public string rightStrafeAnim;
        [Tooltip("Transition time from current animation to the strafing animation.")]
        public float strafeAnimT = 0.25f;
        [Tooltip("Set all the layers you want to avoid when strafing that includes other Blaze AI agents.")]
        public LayerMask strafeLayersToAvoid;


        [Tooltip("When the AI moves to player location in attack state like in Hit or in a chase and finds no enemy the AI will search the radius.")]
        public bool searchLocationRadius;
        [Tooltip("The amount of time to pass in seconds before starting the search.")]
        public float timeToStartSearch = 2;
        [Range(1, 10), Tooltip("The number of points to randomly search.")]
        public int searchPoints = 3;
        [Tooltip("The animation name to play when reaching the search point.")]
        public string searchPointAnim;
        [Tooltip("The amount of time to wait in each search point.")]
        public float pointWaitTime = 3;
        [Tooltip("The animation to play when searching has finished.")]
        public string endSearchAnim;
        [Min(0), Tooltip("The amount of time (seconds) the animation should play for.")]
        public float endSearchAnimTime = 3;
        public float searchAnimsT = 0.25f;

        public bool playAudioOnSearchStart;
        public bool playAudioOnSearchEnd;


        [Tooltip("When the AI is in attack state and there's no hostile at the end location, this animation will play and after the animation time passes the AI will return to alert patrolling. Only works if search empty location is disabled.")]
        public string returnPatrolAnim;
        public float returnPatrolAnimT = 0.25f;
        [Min(0), Tooltip("The duration of the animation after target disappearance to return to alert patrolling.")]
        public float returnPatrolTime = 3f;
        public bool playAudioOnReturnPatrol;



        public enum FirstSightDecision {
            AlwaysAttack,
            TakeCover,
            Randomize
        }

        public enum CoverBlownDecision {
            AlwaysAttack,
            TakeCover,
            Randomize
        }

        public enum AttackEnemyCover {
            AlwaysAttackCover,
            AlwaysAttackEnemy,
            Randomize
        }
        

        #region BEHAVIOUR VARS
        
        public Transform enemyCover { get; private set; }
        public Vector3 hitPoint { get; private set; }
        
        BlazeAI blaze;
        AlertStateBehaviour alertStateBehaviour;
        BlazeAIEnemyManager enemyManager;
        GoingToCoverBehaviour gtcBehaviour;
        Transform previousEnemy;
        Collider enemyColl;

        bool targetChangedTag;
        bool abortStrafe;
        bool sawEnemyOnce;
        bool startTimeUntilAttack;
        bool isStrafeWait;
        bool isStrafing;
        bool isAttacking;
        bool singleShotPass;
        bool shouldAttackCover;
        bool isMovingBackwards;
        bool changeStrafeDir;
        bool braveWait;
        bool shootAnimTriggered;
        bool calculatedLastEnemyPos;
        bool isSearching;
        bool returnPatrolAudioPlayed;

        float _callOthers = 0;
        float _timeToReturnAlert = 0;
        float _timeUntilAttack;
        float timeUntilAttackElapsed;
        float _totalShootTime;
        float _totalShootTimer = 0;
        float _strafeTime;
        float _strafeTimer;
        float _strafeWaitTime;
        float _strafeWaitTimer = 0;
        float _singleShotTimer = 0;
        float _shootAnimTCompleteTimer = 0;
        float searchTimeElapsed = 0;

        int checkPathFrames = 5;
        int checkPathElapsed = 0;
        int strafingDir = 0;
        int strafeCheckPathElapsed = 0;
        int agentPriority;
        int searchIndex = 0;

        Vector3 lastEnemyPos;
        Vector3 searchLocation;

        #endregion
        
        #region UNITY METHODS

        void Start()
        {
            blaze = GetComponent<BlazeAI>();
            alertStateBehaviour = GetComponent<AlertStateBehaviour>();
            gtcBehaviour = GetComponent<GoingToCoverBehaviour>();
            agentPriority = blaze.navmeshAgent.avoidancePriority;


            // force shut if not the same state
            if (blaze.state != BlazeAI.State.attack) {
                enabled = false;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (showCallRadius) {
                ShowCallRadius();
            }
        }

        void OnValidate()
        {
            if (distanceFromEnemy < moveBackwardsDistance) {
                moveBackwardsDistance = distanceFromEnemy - 0.5f;
            }

            if (moveBackwardsDistance >= distanceFromEnemy) {
                moveBackwardsDistance = distanceFromEnemy - 0.5f;
            }
        }

        void OnDisable()
        {
            blaze.navmeshAgent.avoidancePriority = agentPriority;
            previousEnemy = null;
            calculatedLastEnemyPos = false;
            returnPatrolAudioPlayed = false;

            ResetSearching();
        }

        void Update()
        {
            // this behaviour should always remove occupation from cover
            gtcBehaviour.RemoveCoverOccupation();
            
            
            if (blaze.friendly) {
                NoTarget();
                return;
            }


            // call other agents to location
            CallOthers();

            // run timer to attack
            TimeUntilAttack();
            
            // attack duration timers
            AttackingTimers();

            // force the strafing and moving backwards flags are always false if properties not enabled
            ValidateFlags();


            if (!singleShotPass) {
                shootAnimTriggered = false;
            }

            
            // if enemy exists -> engage
            if (blaze.enemyToAttack) {
                calculatedLastEnemyPos = false;
                Engage(blaze.enemyToAttack.transform, braveWait);
                return;
            }
            
            
            // REACHING THIS POINT MEANS ENEMY DOESN'T EXIST

            NoTargetInView();


            // check if called by another agent
            if (blaze.checkEnemyPosition != Vector3.zero) {
                GoToLocation(blaze.checkEnemyPosition);
                return;
            }


            // search within radius of empty location
            if (isSearching) {
                if (blaze.MoveTo(searchLocation, alertStateBehaviour.moveSpeed, alertStateBehaviour.turnSpeed, alertStateBehaviour.moveAnim)) {
                    // stay idle
                    if (!IsSearchPointIdleFinished()) {
                        return;
                    }


                    if (searchIndex < searchPoints) {
                        SetSearchPoint();
                        return;
                    }


                    // reaching this line means the AI has went through all search points and is time to exit
                    EndSearchExit();
                    return;
                }

                return;
            }


            // check if target has changed the tag to something non-hostile
            if (targetChangedTag) {
                NoTarget();
                return;
            }


            // if tag hasn't changed go check last enemy location
            

            // go to last location if reachable
            if (blaze.isPathReachable) {
                if (!calculatedLastEnemyPos) {
                    calculatedLastEnemyPos = true;
                    lastEnemyPos = blaze.RandomSpherePoint(blaze.enemyColPoint);
                }


                if (lastEnemyPos == Vector3.zero) {
                    GoToLocation(blaze.enemyColPoint);
                    return;
                }
                

                GoToLocation(lastEnemyPos);
                return;
            }

            
            // if not -> exit state
            NoTarget();
        }

        #endregion

        #region ATTACK

        void Engage(Transform target, bool foundNoCoverPass=false)
        {
            _timeToReturnAlert = 0;


            // check target tag hasn't changed to something non-hostile
            if (System.Array.IndexOf(blaze.vision.hostileTags, target.tag) < 0) {
                targetChangedTag = true;
                return;
            }

            targetChangedTag = false;


            // add the enemy manager to the hostile
            AddEnemyManager(target, false);


            float minDistance = 0f;
            float backupDistTemp = 0f;

            // set the min distance according to attack and/or enemy manager call
            float[] setDistances = SetDistances();
            minDistance = setDistances[0];
            backupDistTemp = setDistances[1];


            // check path is reachable every 5 frames -> if not, stay idle
            checkPathElapsed++;

            if (checkPathElapsed >= checkPathFrames) {
                checkPathElapsed = 0;
                
                if (!blaze.IsPathReachable(target.position)) {
                    if (blaze.distanceToEnemySqrMag > minDistance * minDistance) {
                        blaze.isAttacking = false;
                        abortStrafe = true;
                        ReachDistance();

                        return;
                    }
                }
            }


            abortStrafe = false;


            // check if AI doesn't need to back away
            if (blaze.distanceToEnemySqrMag > (backupDistTemp * backupDistTemp)) {
                isMovingBackwards = false;
                
                
                // if distance is bigger than min distance -> move
                if (blaze.distanceToEnemySqrMag > (minDistance * minDistance)) {
                    MoveToTarget(target.position);
                    return;
                }

                
                // if reached min distance and is attacking true -> launch attack
                if (blaze.isAttacking) {
                    if (CanSeeTarget(true)) {
                        Attack();
                        return;
                    }
                    
                    MoveToTarget(target.position);
                    return;
                }

                
                abortStrafe = false;


                if (foundNoCoverPass) {
                    IdleStance();
                    return;
                }

                
                ReachDistance();
                return;
            }


            // STARTING FROM HERE MEANS THE AI SHOULD BACK AWAY
            

            // only backup if seeing target
            if (!CanSeeTarget(blaze.isAttacking)) {
                MoveToTarget(target.position);
                return;
            }


            // check if target will be seen from backup point
            if (CheckIfTargetSeenFromPoint(transform.position - (transform.forward * blaze.centerPosition.y))) {
                // sets isMovingBackwards to true 
                MoveBackwards(target.position);
            }

            
            if (blaze.isAttacking) {
                Attack(isMovingBackwards);
                return;
            }
            
            
            ReachDistance();
        }
        
        // what to do when the AI is in good distance to it's enemy
        void ReachDistance()
        {
            if (sawEnemyOnce) {
                // if brave meter is less or equal to zero -> go to cover
                if (braveMeter <= 0) {
                    braveWait = false;
                    blaze.SetState(BlazeAI.State.goingToCover);
                    return;
                }


                // if braveness meter is at 10 -> go to brave wait no need to calculate chance
                if (braveMeter >= 10) {
                    braveWait = true;
                    return;
                }


                // calculate the odds and chance of going to cover
                int odds = 10 - braveMeter;
                int chanceOfTakingCover = Random.Range(1, 10);

                if (chanceOfTakingCover > odds) {
                    braveWait = true;
                    return;
                }

                if (chanceOfTakingCover <= odds) {
                    braveWait = false;
                    blaze.SetState(BlazeAI.State.goingToCover);
                    return;
                }
            }
            

            sawEnemyOnce = true;
            blaze.animManager.Play(idleAnim, idleMoveT);
            

            // first sight decision making
            if (firstSightDecision == FirstSightDecision.AlwaysAttack) {
                blaze.Attack();
            }
            else if (firstSightDecision == FirstSightDecision.TakeCover) {
                blaze.SetState(BlazeAI.State.goingToCover);
            }
            else {
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    blaze.SetState(BlazeAI.State.goingToCover);
                    return;
                }
                
                blaze.Attack();
            }
        }

        // move the AI to target position
        void MoveToTarget(Vector3 pos)
        {
            blaze.MoveTo(pos, moveSpeed, turnSpeed, moveAnim, idleMoveT, "front", distanceFromEnemy);
            changeStrafeDir = false;
            _shootAnimTCompleteTimer = 0;
        }

        // set the min distance for the AI according to conditions
        float[] SetDistances()
        {
            float[] arr = new float[2];
            float minDistance = 0f;
            float backupDistTemp = 0f;


            // set the min distance according to attack and/or enemy manager call            
            if (enemyManager.callEnemies) {
                if (blaze.isAttacking) {
                    minDistance = attackDistance;
                    
                    if (moveBackwards && moveBackwardsAttack) backupDistTemp = moveBackwardsDistance;
                    else backupDistTemp = 0f;

                    arr[0] = minDistance;
                    arr[1] = backupDistTemp;

                    return arr;
                }
            }
            else {
                blaze.isAttacking = false;
                AllAttackStopReset();
            }


            if (isStrafing) minDistance = distanceFromEnemy + 1;
            else minDistance = distanceFromEnemy;


            if (moveBackwards) backupDistTemp = moveBackwardsDistance;

        
            arr[0] = minDistance;
            arr[1] = backupDistTemp;

            return arr;
        }

        // timer to attack
        public void TimeUntilAttack()
        {
            // if no enemy, blaze not flagged to attack or enemy manager set to not call enemies -> return
            if (!blaze.enemyToAttack || blaze.isAttacking) {
                startTimeUntilAttack = false;
                timeUntilAttackElapsed = 0;
                return;
            }

            
            if (enemyManager) {
                if (!enemyManager.callEnemies) {
                    return;
                }
            }


            // set the random time until attack to count up to
            if (!startTimeUntilAttack) {
                _timeUntilAttack = Random.Range(shootEvery.x, shootEvery.y);
                startTimeUntilAttack = true;
            }


            // increment time to attack timer
            timeUntilAttackElapsed += Time.deltaTime;

            if (timeUntilAttackElapsed >= _timeUntilAttack) {
                TurnToAttack();
            }
        }

        // tell this AI to attack
        void TurnToAttack()
        {
            startTimeUntilAttack = false;
            timeUntilAttackElapsed = 0;


            // attack cover or not
            if (attackEnemyCover == AttackEnemyCover.AlwaysAttackCover) {
                shouldAttackCover = true;
            }
            else if (attackEnemyCover == AttackEnemyCover.AlwaysAttackEnemy) {
                shouldAttackCover = false;
            }
            else {
                int rand = Random.Range(0, 2);
                
                if (rand == 0) {
                    shouldAttackCover = true;
                }
                else {
                    shouldAttackCover = false;
                }
            }


            blaze.Attack();
        }
        
        // actual attack (shooting) method
        // movingBackwards parameter => attacking while moving backwards
        void Attack(bool movingBackwards = false)
        {
            // make sure the shoot anim transition is complete
            if (!isStrafing) {
                _shootAnimTCompleteTimer += Time.deltaTime;
                
                if (_shootAnimTCompleteTimer < shootingAnimT + 0.15f) {
                    ShootAnim();
                    return;
                }
            }


            // if not attacking while moving backwards
            if (!movingBackwards) {
                // if strafe enabled 
                if (strafe) {
                    Strafe();
                }
            }


            if (singleShotPass) {
                // if waiting to strafe -> (NOT STRAFING YET) then play the shooting anim
                if (!isStrafing && !movingBackwards) {
                    ShootAnim();
                }

                return;
            }
            else {
                if (!isAttacking) {
                    _totalShootTime = Random.Range(totalShootTime.x, totalShootTime.y);
                }

                shootAnimTriggered = false;
            }


            // invoke the selected attack functions
            shootEvent.Invoke();

            
            // play shoot animation
            if (!isStrafing) {
                ShootAnim();
            }


            // flags that we initiated the attack
            isAttacking = true;


            // flags the single shot (we do several shots per attack)
            singleShotPass = true;
        }
        
        // track the timers responsible for attacks
        void AttackingTimers()
        {
            if (!isAttacking) {
                return;
            }


            // if not attacking return
            if (!blaze.isAttacking) {
                AllAttackStopReset();
                return;
            }


            // rotate to face target when attacking
            blaze.RotateTo(blaze.enemyColPoint, 7);


            // total attack duration timer
            _totalShootTimer += Time.deltaTime;
            if (_totalShootTimer >= _totalShootTime) {
                _totalShootTimer = 0;
                StopAttack();
                return;
            }
            

            if (!singleShotPass) {
                return;
            }


            // single shot timer
            _singleShotTimer += Time.deltaTime;
            if (_singleShotTimer >= singleShotDuration) {
                singleShotPass = false;
                _singleShotTimer = 0;
            }
        }

        // stop the shooting cycle
        void StopAttack()
        {
            blaze.isAttacking = false;
            ResetAttackFlags();
            braveWait = false;
        }

        // reset all the attack flags back to normal
        void ResetAttackFlags()
        {
            isAttacking = false;
            singleShotPass = false;

            isStrafing = false;
            isStrafeWait = false;
            
            _shootAnimTCompleteTimer = 0;
        }

        // stop and reset attack if blaze.StopAttack() used or enemy manager Call Enemies -> set to false
        void AllAttackStopReset() 
        {
            ResetAttackFlags();
            singleShotPass = false;
            _totalShootTimer = 0f;
            _singleShotTimer = 0f;
            _shootAnimTCompleteTimer = 0;
        }

        // get the target's collider
        Collider GetTargetCollider(Transform currentEnemy)
        {
            if (currentEnemy == null) {
                return null;
            }

            if (enemyColl != null && currentEnemy == previousEnemy) {
                return enemyColl;
            }

            Collider coll = blaze.enemyToAttack.GetComponent<Collider>();
            return coll;
        }

        // get the cover the target is hiding behind
        Transform GetTargetCover(Vector3 targetPos)
        {
            // cache the target's collider
            enemyColl = GetTargetCollider(blaze.enemyToAttack.transform);

            if (!enemyColl) return null;


            Collider[] coll = new Collider[5];
            int collNum = Physics.OverlapSphereNonAlloc(targetPos, (enemyColl.bounds.size.x + enemyColl.bounds.size.z) + 0.3f, coll, gtcBehaviour.coverLayers);

            float bestDotProd = -1;
            Transform bestCover = null;

            // get the best cover using dot product
            for (int i=0; i<collNum; i+=1) {
                float dotProd = Vector3.Dot((coll[i].transform.position - transform.position).normalized, transform.forward);
                if (dotProd > bestDotProd) {
                    bestDotProd = dotProd;
                    bestCover = coll[i].transform;
                }
            }

            // get the closest one if dot product way didn't work
            if (bestCover == null && collNum > 0) {
                bestCover = coll[0].transform;
            }

            // if AI is hiding on the opposite side of the same cover
            if (bestCover == gtcBehaviour.lastCover) {
                bestCover = null;
            }
            
            return bestCover;
        }

        // check whether target is seen or not and cache the hit point
        bool CanSeeTarget(bool onAttacking = false)
        {
            RaycastHit hit;
            Vector3 targetDir = blaze.enemyToAttack.transform.position - transform.position;


            int layers;

            if (onAttacking) {
                layers = layersCheckOnAttacking | blaze.vision.layersToDetect | blaze.vision.hostileAndAlertLayers | gtcBehaviour.coverLayers;
            }
            else {
                layers = blaze.vision.layersToDetect | blaze.vision.hostileAndAlertLayers | gtcBehaviour.coverLayers;
            }


            bool clearView = false;

            if (Physics.SphereCast(transform.position, blaze.navmeshAgent.radius/2, targetDir, out hit, Mathf.Infinity, layers)) {
                // if sphere cast hits the same AI collider
                if (transform.IsChildOf(hit.transform)) {
                    return true;
                }


                // check if target cover should be a target
                if (shouldAttackCover) {
                    enemyCover = GetTargetCover(blaze.enemyToAttack.transform.position);

                    if (enemyCover) {
                        if (enemyCover.IsChildOf(hit.transform)) {
                            clearView = true;
                        }
                    }

                    if (blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                        clearView = true;
                    }
                }
                else {
                    if (blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                        clearView = true;
                    }
                }
            }


            if (clearView) hitPoint = hit.point;

            return clearView;
        }

        // add enemy manager to target
        void AddEnemyManager(Transform currentEnemy, bool addToManager = true)
        {
            if (currentEnemy == null) {
                return;
            }

            if (currentEnemy == previousEnemy) {
                return;
            }

            enemyManager = currentEnemy.GetComponent<BlazeAIEnemyManager>();
            if (enemyManager == null) {
                currentEnemy.gameObject.AddComponent<BlazeAIEnemyManager>();
                enemyManager = currentEnemy.GetComponent<BlazeAIEnemyManager>();
            }

            if (addToManager) {
                enemyManager.AddEnemy(blaze);
            }
            
            previousEnemy = currentEnemy;
        }

        void IdleStance()
        {
            isMovingBackwards = false;


            if (strafe) {
                if (enabled) {
                    // check if strafing has already been fired to avoid double trigger of movement
                    if (!isStrafing && !isStrafeWait) {
                        Strafe();
                        return;
                    }
                    
                    if (braveWait) {
                        Strafe();
                        return;
                    }

                    return;
                }
                
                // IF THIS BLOCK FIRES MEANS GOING TO COVER BEHAVIOUR IS CALLING
                Strafe();
                return;
            }

            
            blaze.RotateTo(blaze.enemyToAttack.transform.position, 7);
            blaze.animManager.Play(idleAnim, idleMoveT);
        }

        #endregion

        #region STRAFING
        
        // prepare the strafe
        void Strafe()
        {
            if (abortStrafe) {
                if (!singleShotPass) {
                    IdleAnim();
                }
                
                return;
            }


            // if waiting for strafe wait (time to pass before strafing)
            if (isStrafeWait) {
                // rotate to enemy always and play idle anim
                if (!singleShotPass) {
                    IdleAnim();
                }

                // count up the strafe wait timer
                StrafeWait();
                
                return;
            }


            // if already strafing continue on with strafe movement
            if (isStrafing) {
                StrafeMovement(strafingDir);
                return;
            }

            
            // IF REACHED THIS POINT THEN THIS IS THE FIRST RUN

            
            // set the time of strafing
            if (strafeTime.x == -1 && strafeTime.y == -1) {
                _strafeTime = Mathf.Infinity;
            }
            else {
                _strafeTime = Random.Range(strafeTime.x, strafeTime.y);
            }


            // set the wait time
            _strafeWaitTime = Random.Range(strafeWaitTime.x, strafeWaitTime.y);


            // check for flag to change current dir
            if (changeStrafeDir) {
                if (strafingDir == 0) strafingDir = 1;
                else strafingDir = 0;

                changeStrafeDir = false;
            }
            else {
                strafingDir = Random.Range(0, 2);
            }

            
            isStrafeWait = true;
        }

        // the actual strafing movement
        void StrafeMovement(int direction)
        {   
            // if agent isn't on navmesh
            if (!blaze.IsPointOnNavMesh(blaze.ValidateYPoint(transform.position), blaze.navmeshAgent.radius)) {
                StopStrafe();
                return;
            }


            RaycastHit hit;
            int layersToHit = blaze.vision.hostileAndAlertLayers | strafeLayersToAvoid | blaze.vision.layersToDetect;

            Vector3 strafePoint = Vector3.zero;
            Vector3 offsetPlayer;
            Vector3 transformDir;
            Vector3 shoulderOffset;

            string strafeAnim;
            string moveDir;

            Vector3 enemyToAttackPos = new Vector3(blaze.enemyToAttack.transform.position.x, transform.position.y, blaze.enemyToAttack.transform.position.z);
            float distanceDiff = Vector3.Distance(enemyToAttackPos, transform.position);


            // if direction is left
            if (direction == 0) {
                offsetPlayer = enemyToAttackPos - transform.position;
                offsetPlayer = Vector3.Cross(offsetPlayer, Vector3.up);

                strafePoint = blaze.ValidateYPoint(offsetPlayer);
                strafePoint = transform.position + new Vector3(strafePoint.x, 0, strafePoint.z + 0.5f);
                strafePoint = strafePoint + (transform.right * (distanceDiff - 1));

                transformDir = -transform.right;

                // to check from an offset if enemy will not be visible
                shoulderOffset = transform.TransformPoint(new Vector3(-1f, 0f, 0f) + blaze.centerPosition);

                // set the anim and move dir
                strafeAnim = leftStrafeAnim;
                moveDir = "left";
            }
            else {
                offsetPlayer = transform.position - enemyToAttackPos;
                offsetPlayer = Vector3.Cross(offsetPlayer, Vector3.up);

                strafePoint = blaze.ValidateYPoint(offsetPlayer);
                strafePoint = transform.position + new Vector3(strafePoint.x, 0, strafePoint.z + 0.5f);
                strafePoint = strafePoint + (-transform.right * (distanceDiff - 1));

                transformDir = transform.right;

                // to check from an offset if enemy will not be visible
                shoulderOffset = transform.TransformPoint(new Vector3(1f, 0f, 0f) + blaze.centerPosition);
                
                // set the anim and move dir
                strafeAnim = rightStrafeAnim;
                moveDir = "right";
            }


            // check if point reachable and has navmesh every 5 frames
            if (strafeCheckPathElapsed >= 5) {
                strafeCheckPathElapsed = 0;
                Vector3 goToPoint = blaze.ValidateYPoint((transform.position + blaze.centerPosition) + (transformDir * (blaze.navmeshAgent.radius * 2 + blaze.navmeshAgent.height)));
                
                if (!blaze.IsPointOnNavMesh(goToPoint, 0.3f)) {
                    ChangeStrafeDirection();
                    return;
                }

                if (!blaze.IsPathReachable(goToPoint)) {
                    ChangeStrafeDirection();
                    return;
                }

            }

            strafeCheckPathElapsed++;


            // check if there's an obstacle in strafe direction
            if (Physics.SphereCast(transform.position + blaze.centerPosition, 0.3f, transformDir, out hit, (blaze.navmeshAgent.radius * 2) + blaze.navmeshAgent.height/2, layersToHit)) {   
                ChangeStrafeDirection();
                return;
            }


            // to check from an offset if enemy (or cover) will not be visible
            if (Physics.Raycast(shoulderOffset, blaze.enemyColPoint - shoulderOffset, out hit, Mathf.Infinity, layersToHit)) {   
                // if should attack target cover
                if (shouldAttackCover) {
                    // check if enemy is hiding behind cover
                    if (enemyCover) {
                        if (!enemyCover.IsChildOf(hit.transform) && !blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                            ChangeStrafeDirection();
                            return;
                        }
                    }
                    else {
                        if (!blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                            ChangeStrafeDirection();
                            return;
                        }
                    }
                }
                else {
                    // if targeting enemy only
                    if (!blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                        ChangeStrafeDirection();
                        return;
                    }
                }
            }


            isStrafing = true;
            blaze.RotateTo(blaze.enemyToAttack.transform.position, 20f);
            blaze.MoveTo(strafePoint, strafeSpeed, 0, strafeAnim, strafeAnimT, moveDir);

            
            _strafeTimer += Time.deltaTime;
            if (_strafeTimer >= _strafeTime) {
                StopStrafe();
            }
        }

        // wait before strafing
        void StrafeWait()
        {
            _strafeWaitTimer += Time.deltaTime;
            
            if (_strafeWaitTimer >= _strafeWaitTime) {
                isStrafing = true;
                isStrafeWait = false;
                _strafeWaitTimer = 0f;
            }
        }

        // stop the strafing
        void StopStrafe()
        {
            isStrafeWait = false;
            _strafeWaitTimer = 0f;

            isStrafing = false;
            _strafeTimer = 0f;
        }

        // change the strafing to the opposite direction
        void ChangeStrafeDirection()
        {   
            changeStrafeDir = true;
            StopStrafe();
        }
        
        #endregion

        #region MOVE BACKWARDS
        
        // back away from target
        void MoveBackwards(Vector3 target)
        {
            Vector3 targetPosition = transform.position - (transform.forward * (blaze.navmeshAgent.height - 0.25f));
            Vector3 backupPoint = blaze.ValidateYPoint(targetPosition);
            RaycastHit hit;
            backupPoint = new Vector3 (backupPoint.x, transform.position.y, backupPoint.z + 0.5f);
            
            int layers = blaze.vision.layersToDetect | agentLayersToCall | gtcBehaviour.coverLayers;


            // check if obstacle is behind
            if (Physics.Raycast(transform.position + blaze.centerPosition, -transform.forward, out hit, blaze.navmeshAgent.radius * 2 + 0.3f, layers)) {
                IdleStance();
                return;
            }
            
            // if point isn't on navmesh
            if (!blaze.IsPointOnNavMesh(backupPoint, 0.3f)) {
                IdleStance();
                return;
            }
            
            // if point isn't reachable
            if (!blaze.IsPathReachable(backupPoint)) {
                IdleStance();
                return;
            }

            
            // if strafing we need further precision to check if moving backwards is possible
            // to prevent disabling strafing to find the AI moves backwards a very neglibile distance which makes it look very bad
            if (isStrafing) {
                float distanceDiff = Vector3.Distance(transform.position, blaze.enemyToAttack.transform.position);
                if (Physics.Raycast(transform.position + blaze.centerPosition, -transform.forward, out hit, moveBackwardsDistance / 1.5f + blaze.navmeshAgent.radius, layers)) {
                    IdleStance();
                    return;
                }
                else {
                    Vector3 checkPoint = new Vector3(backupPoint.x, transform.position.y, backupPoint.z + (moveBackwardsDistance / 2f));
                    if (!blaze.IsPathReachable(checkPoint)) {
                        IdleStance();
                        return;
                    }
                }
            }


            // cancel strafing when backing away
            StopStrafe();
            

            // back away
            blaze.RotateTo(target, turnSpeed);
            blaze.MoveTo(backupPoint, moveBackwardsSpeed, 0f, moveBackwardsAnim, moveBackwardsAnimT, "backwards");


            isMovingBackwards = true;
        }
        
        #endregion

        #region CALL OTHERS
        
        // call nearby agents to target location
        public void CallOthers()
        {
            // if call others isn't enabled or no target
            if (!callOthers || !blaze.enemyToAttack) {
                return;
            }


            // time to pass before firing
            _callOthers += Time.deltaTime;
            if (_callOthers < callOthersTime) {
                return;
            }

            _callOthers = 0;


            Collider[] callOthersColl = new Collider[20];
            int callOthersNum = Physics.OverlapSphereNonAlloc(transform.position, callRadius, callOthersColl, agentLayersToCall);

            for (int i=0; i<callOthersNum; i+=1) {
                BlazeAI script = callOthersColl[i].GetComponent<BlazeAI>();
                AttackStateBehaviour attackBehaviour = callOthersColl[i].GetComponent<AttackStateBehaviour>();
                CoverShooterBehaviour coverShooterBehaviour = callOthersColl[i].GetComponent<CoverShooterBehaviour>();

                if (callOthersColl[i].transform.IsChildOf(transform)) {
                    continue;
                }


                // if doesn't have Blaze AI
                if (!script) {
                    continue;
                }


                // if doesn't have this AttackStateBehaviour script
                if (!attackBehaviour && !coverShooterBehaviour) {
                    continue;
                }


                // check if doesn't receive calls
                if (attackBehaviour) {
                    if (!attackBehaviour.receiveCallFromOthers) {
                        continue;
                    }
                }
                else {
                    if (!coverShooterBehaviour.receiveCallFromOthers) {
                        continue;
                    }
                }


                // if agent already has a target then don't call
                if (script.enemyToAttack) {
                    continue;
                }


                // if other agent has seen the target after this agent then don't call
                if (script.captureEnemyTimeStamp > blaze.captureEnemyTimeStamp) {
                    continue;
                }
                
                
                // set the check enemy position of the other agents to target position
                script.checkEnemyPosition = blaze.RandomSpherePoint(blaze.enemyColPoint);

                // turn the agents to attack state
                script.TurnToAttackState();
            }
        }

        #endregion

        #region GETTING CALLED BY OTHERS
        
        void GoToLocation(Vector3 location)
        {
            sawEnemyOnce = true;
            
            // moves AI to location and returns true when reaches location
            if (blaze.MoveTo(location, moveSpeed, turnSpeed, moveAnim, idleMoveT)) {
                NoTarget();
            }
        }
        
        #endregion

        #region MISC

        // fired by the GoingToCoverBehaviour script when no covers found
        public void FoundNoCover()
        {
            Engage(blaze.enemyToAttack.transform, true);
            CallOthers();
        }

        // show the call others radius in scene view
        void ShowCallRadius()
        {
            if (!callOthers) {
                return;
            }

            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, callRadius);
        }

        void ResetTimers()
        {
            _callOthers = 0;
            _totalShootTimer = 0;
            _singleShotTimer = 0;
            timeUntilAttackElapsed = 0;
        }

        void NoTargetInView()
        {
            StopAttack();
            StopStrafe();

            sawEnemyOnce = false;
            gtcBehaviour.RemoveCoverOccupation();
        }

        public void ResetTimeUntilAttack()
        {
            timeUntilAttackElapsed = 0;
        }

        // check if target is seen from a certain position
        public bool CheckIfTargetSeenFromPoint(Vector3 point)
        {
            Vector3 dir = blaze.enemyToAttack.transform.position - point;
            RaycastHit hit;
            int layers = blaze.vision.layersToDetect | blaze.vision.hostileAndAlertLayers | gtcBehaviour.coverLayers;


            if (Physics.SphereCast(point, 0.2f, dir, out hit, Mathf.Infinity, layers)) {
                if (blaze.enemyToAttack.transform.IsChildOf(hit.transform)) {
                    return true;
                }
            }


            return false;
        }

        // force the strafing and moving backwards flags are always false if properties not enabled
        void ValidateFlags()
        {
            if (!strafe) {
                isStrafeWait = false;
                isStrafing = false;
            }

            if (!moveBackwards) {
                isMovingBackwards = false;
            }
        }

        void ResetEnemyManager()
        {
            if (enemyManager) {
                previousEnemy = null;
                enemyManager.RemoveEnemy(blaze);
            }
        }

        #endregion

        #region ANIMATIONS
        
        // idle animation
        void IdleAnim()
        {
            blaze.RotateTo(blaze.enemyToAttack.transform.position, 7);
            blaze.animManager.Play(idleAnim, idleMoveT);
        }

        // shoot animation
        void ShootAnim()
        {
            blaze.RotateTo(blaze.enemyToAttack.transform.position, 7);

            if (shootAnimTriggered) {
                return;
            }

            blaze.animManager.Play(shootingAnim, shootingAnimT, true);
            shootAnimTriggered = true;
        }
        
        #endregion

        #region RETURN PATROL/ALERT && SEARCHING
        
        // reached location and no hostile found
        void NoTarget()
        {
            if (blaze.companionMode) {
                ReturnToAlert();
            }


            // search empty location
            if (searchLocationRadius) {
                blaze.animManager.Play(idleAnim, returnPatrolAnimT);
                searchTimeElapsed += Time.deltaTime;

                if (searchTimeElapsed >= timeToStartSearch) {
                    PlaySearchStartAudio();
                    SetSearchPoint();
                    
                    isSearching = true;
                    return;
                }

                return;
            }


            PlayReturnPatrolAudio();
            ReturnToAlertIdle();
            ResetTimers();

            
            _timeToReturnAlert += Time.deltaTime;
            if (_timeToReturnAlert >= returnPatrolTime) {
                ReturnToAlert();
            }
        }


        // play return animation
        void ReturnToAlertIdle()
        {
            blaze.SetState(BlazeAI.State.returningToAlert);
            
            
            if (returnPatrolAnim.Length == 0) {
                blaze.animManager.Play(idleAnim, returnPatrolAnimT);
            }
            else {
                blaze.animManager.Play(returnPatrolAnim, returnPatrolAnimT);
            }
            

            ResetEnemyManager();
        }
        

        // exit attack state and return to alert
        void ReturnToAlert()
        {
            _timeToReturnAlert = 0;
            blaze.SetState(BlazeAI.State.alert);
        }


        // play start search audio
        void PlayReturnPatrolAudio()
        {
            // if audio already played -> return
            if (returnPatrolAudioPlayed) {
                return;
            }


            if (!playAudioOnReturnPatrol) {
                return;
            }


            if (blaze.IsAudioScriptableEmpty()) {
                return;
            }


            blaze.PlayAudio(blaze.audioScriptable.GetAudio(AudioScriptable.AudioType.ReturnPatrol));
            returnPatrolAudioPlayed = true;
        }


        // set the next search point
        void SetSearchPoint()
        {
            searchLocation = blaze.RandomSpherePoint(transform.position, (blaze.navmeshAgent.height * 2) + 2);
            
            // make sure never returns 0
            if (searchLocation == Vector3.zero) {
                SetSearchPoint();
                return;
            }
            
            searchIndex++;
            searchTimeElapsed = 0;
        }


        // returns whether the idle time has finished in the search point or not
        bool IsSearchPointIdleFinished()
        {
            blaze.animManager.Play(searchPointAnim, searchAnimsT);

            searchTimeElapsed += Time.deltaTime;
            if (searchTimeElapsed >= pointWaitTime) {
                return true;
            }

            return false;
        }


        // play start search audio
        void PlaySearchStartAudio()
        {
            if (!playAudioOnSearchStart) {
                return;
            }


            if (blaze.IsAudioScriptableEmpty()) {
                return;
            }


            blaze.PlayAudio(blaze.audioScriptable.GetAudio(AudioScriptable.AudioType.SearchStart));
        }


        // play search end audio
        void PlaySearchEndAudio()
        {
            if (!playAudioOnSearchEnd) {
                return;
            }


            if (blaze.IsAudioScriptableEmpty()) {
                return;
            }


            blaze.PlayAudio(blaze.audioScriptable.GetAudio(AudioScriptable.AudioType.SearchEnd));
        }


        // exit the search and distracted state
        void EndSearchExit()
        {
            blaze.animManager.Play(endSearchAnim, searchAnimsT);
            PlaySearchEndAudio();

            searchTimeElapsed += Time.deltaTime;

            if (searchTimeElapsed >= endSearchAnimTime) {
                ResetEnemyManager();
                ReturnToAlert();
            }
        }


        void ResetSearching()
        {
            searchIndex = 0;
            isSearching = false;
            searchTimeElapsed = 0f;
        }
        
        #endregion
    }
}
