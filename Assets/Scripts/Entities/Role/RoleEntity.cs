using System;
using UnityEngine;

namespace Sample2D {

    // 角色
    // 角色可以施放技能
    // 动作是根据技能来的
    // 先不写技能, 先用一个按键写死, 普攻按键
    // √ Role -> Skillor -> Actionor
    // 偷懒 Role -> Actionor
    public class RoleEntity : MonoBehaviour {

        [SerializeField] float moveSpeed;
        [SerializeField] float jumpForce;
        [SerializeField] float fallingSpeed;
        [SerializeField] float fallingRaiseSpeed;

        bool isJump;
        bool isGround;

        // COMPONENT
        RoleFootComponent footComponent;

        // ==== 动作 ====
        RoleActionorComponent actionorComponent;
        public RoleActionorComponent ActionorComponent => actionorComponent;

        // ==== 渲染 ====
        SpriteRenderer mesh;
        public SpriteRenderer Mesh => mesh;

        // ==== 物理 ====
        Rigidbody2D rb;
        BoxCollider2D bodyBox;

        // ==== 动画 ====
        Animator animator;

        void Awake() {

            moveSpeed = 5.5f;
            jumpForce = 12f;
            fallingSpeed = 40f;
            fallingRaiseSpeed = 10f;

            isJump = false;
            isGround = true;

            rb = GetComponent<Rigidbody2D>();
            bodyBox = GetComponent<BoxCollider2D>();

            mesh = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = mesh.GetComponent<Animator>();

            footComponent = GetComponentInChildren<RoleFootComponent>();
            footComponent.OnEnterGroundHandle += OnPhysicsEnterGround;

            actionorComponent = GetComponentInChildren<RoleActionorComponent>();

            Debug.Assert(rb != null);
            Debug.Assert(bodyBox != null);
            Debug.Assert(animator != null);
            Debug.Assert(actionorComponent != null);

        }

        // ==== LOCOL MOTION ====
        // 左右移动
        public void LocomotionMove(Vector2 moveAxis) {

            float vertical = rb.velocity.y;

            float xOffset = moveAxis.x; // 左 -1 , 左上 -0.71
            if (xOffset < 0) {
                xOffset = -1;
            } else if (xOffset > 0) {
                xOffset = 1;
            }

            rb.velocity = new Vector2(xOffset * moveSpeed, vertical);

            // 改变面向
            if (xOffset != 0) {
                mesh.transform.localScale = new Vector3(xOffset, 1, 1);
            }

            // 移动动画
            float animSpeed = Math.Abs(rb.velocity.x);
            animator.SetFloat("speed",  animSpeed);

        }

        // 跳
        public void LocomotionJump(float jumpAxis) {

            if (jumpAxis <= 0 || isJump) {
                return;
            }

            isJump = true;
            isGround = false;

            float horizontal = rb.velocity.x;

            rb.velocity = new Vector2(horizontal, jumpForce); // +12f

            // 跳动画
            animator.SetBool("isJump", isJump);
            animator.SetBool("isGround", isGround);

        }

        // 下落
        public void LocomotionFalling(float jumpAxis, float fixedDeltaTime) {

            // velo.y
            float x = rb.velocity.x;
            float y = rb.velocity.y;

            if (jumpAxis > 0) {

                // 按住跳时, 跳得更高(下落加速度更慢)
                // y - (20f - 4.5f) = -16.5f * fixedDeltaTime
                y -= (fallingSpeed - fallingRaiseSpeed) * fixedDeltaTime;

            } else {

                // 没按住跳时, 跳得更低(下落加速度更快)
                // y - 20f = -20f * fixedDeltaTime
                y -= fallingSpeed * fixedDeltaTime;

            }

            rb.velocity = new Vector2(x, y);

        }

        // ==== ANIMATION ====
        public void AnimPlayMelee() {
            animator.Play("anim_role_melee");
        }

        // ==== PHYSICS LOGIC ====
        public void PhysicsSetBodyBoxActivation(bool isActive) {
            bodyBox.enabled = isActive;
        }

        // ==== PHYSICS EVENT ====
        // 落地检测
        void OnPhysicsEnterGround() {

            isJump = false;
            isGround = true;

            // 落地动画
            animator.SetBool("isJump", isJump);
            animator.SetBool("isGround", isGround);

        }

    }

}