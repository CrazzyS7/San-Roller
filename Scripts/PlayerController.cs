using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem mSmokePtcl;

    private Vector3 mSmokePtclOffset = new Vector3(0, 1.0f, 0);
    private Vector3 mNextCollisionPosition = Vector3.zero;
    private Vector2 mSwipePosCurrentFrame = Vector2.zero;
    private Vector2 mSwipePosLastFrame = Vector2.zero;
    private Vector2 mCurrentSwipe = Vector2.zero;
    private readonly float mMaxDistance = 100.0f;
    private Vector3 mTravelDir = Vector3.zero;
    private readonly float mSpeed = 15.0f;
    private readonly int mMinSwipe = 200;
    private GameManager mGameManager;
    private bool mIsMoving = false;
    private Rigidbody mPlayerRB;
    private Color mSolveColor;

    private void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mSolveColor = Random.ColorHSV(0.7f, 1);
        GetComponent<MeshRenderer>().material.color = mSolveColor;
        mPlayerRB = GetComponent<Rigidbody>();
        return;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = transform.position;
        mSmokePtcl.transform.position = playerPos - mSmokePtclOffset;

        if (mIsMoving)
        {
            mSmokePtcl.Play();
            mPlayerRB.velocity = mTravelDir * mSpeed;
        }

        int iter = 0;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        while(iter < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[iter].transform.GetComponent<GroundPiece>();
            if(ground && !ground.IsColored)
            {
                ground.ChangeColor(mSolveColor);
            }
            iter++;
        }
        
        if(mNextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, mNextCollisionPosition) < 1)
            {
                mIsMoving = false;
                mTravelDir = Vector3.zero;
                mNextCollisionPosition = Vector3.zero;
            }
        }
        
        if(mIsMoving)
        {
            return;
        }

        mSmokePtcl.Stop();

        if (!mGameManager.IsGameOver)
        {
            if (Input.GetMouseButton(0))
            {
                mSwipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                if (mSwipePosLastFrame != Vector2.zero)
                {
                    mCurrentSwipe = mSwipePosCurrentFrame - mSwipePosLastFrame;
                    if (mCurrentSwipe.sqrMagnitude < mMinSwipe)
                    {
                        return;
                    }

                    mCurrentSwipe.Normalize();
                    if (mCurrentSwipe.x > -0.5f && mCurrentSwipe.x < 0.5)
                    {
                        //Up or Down
                        SetDestination(mCurrentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                    }

                    if (mCurrentSwipe.y > -0.5 && mCurrentSwipe.y < 0.5)
                    {
                        //Left or Right
                        SetDestination(mCurrentSwipe.x > 0 ? Vector3.right : Vector3.left);
                    }
                }
                mSwipePosLastFrame = mSwipePosCurrentFrame;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            mSwipePosLastFrame = Vector2.zero;
            mCurrentSwipe = Vector2.zero;
        }
        return;
    }

    private void SetDestination(Vector3 _direction)
    {
        mTravelDir = _direction;
        if(Physics.Raycast(transform.position, mTravelDir, out RaycastHit hit, mMaxDistance))
        {
            mNextCollisionPosition = hit.point;
        }
        mIsMoving = true;
        return;
    }
}
