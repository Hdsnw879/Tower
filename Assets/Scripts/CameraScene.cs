using UnityEngine;

public class CameraScene : MonoBehaviour
{
    public float orbitSpeed; // Tốc độ quay của camera xung quanh điểm mục tiêu
    private Transform target; // Đối tượng mà camera sẽ nhìn về và quay quanh
    StartScene st;
    void Start()
    {
        st = GameObject.Find("StartScene").GetComponent<StartScene>();
        target = st.thatMidCell.transform;
    }
    void Update()
    {
        // Kiểm tra xem đã gán đối tượng target trong Inspector chưa
        if (target == null)
        {
            
            return;
        }
        float distance = Vector3.Distance(transform.position,target.position);
        
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

        // Lấy hướng từ camera đến điểm mục tiêu
        Vector3 lookDirection = (target.position - transform.position).normalized;

        // Quay camera nhìn về điểm mục tiêu
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
    }
}
