using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;




public class AgentController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.localPosition;
    }
    public override void OnEpisodeBegin()
    {
        //Agent

        transform.localPosition = new Vector3(UnityEngine.Random.Range(-5f, 3f), 0.3f, UnityEngine.Random.Range(-3f, 9f));

        //Pellet

        target.localPosition = new Vector3(UnityEngine.Random.Range(-5f, 3f), 0.6f, UnityEngine.Random.Range(-3f, 9f));

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];


        Vector3 velocity=new Vector3(moveX, 0f,moveZ);
        velocity = velocity.normalized * Time.deltaTime* moveSpeed;
        transform.localPosition += velocity;
        if (transform.localPosition.y < 0)
        {
            // Yere çarpınca ceza al
            AddReward(-1.0f);
            // Başlangıç pozisyonuna geri dön
            transform.localPosition = initialPosition;
            EndEpisode();

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Pellet")
        {
            AddReward(2.0f);
            EndEpisode();
        }

    }
}
