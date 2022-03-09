//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Diagnostics;
using System.Globalization;
using System.IO;
using UnityEngine;
using Windows.Kinect;

/// <summary>
/// Interaction logic for MainWindow
/// </summary>
public class KinectData {

    /// <summary>
    /// Constant for clamping Z values of camera space points from being negative
    /// </summary>
    private const float InferredZPositionClamp = 0.1f;

    /// <summary>
    /// Active Kinect sensor
    /// </summary>
    private KinectSensor kinectSensor = null;

    /// <summary>
    /// Coordinate mapper to map one type of point to another
    /// </summary>
    private CoordinateMapper coordinateMapper = null;

    /// <summary>
    /// Reader for body frames
    /// </summary>
    private BodyFrameReader bodyFrameReader = null;

    /// <summary>
    /// Array for the bodies
    /// </summary>
    private Body[] bodies = null;

    private List<Body> activeBodies = new List<Body>();

    /// <summary>
    /// Current status text to display
    /// </summary>
    private string statusText = null;


    public List<Body> getActiveBodies() {
        return activeBodies;
    }

    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    public void Start() {
        // one sensor is currently supported
        this.kinectSensor = KinectSensor.GetDefault();

        // get the coordinate mapper
        this.coordinateMapper = this.kinectSensor.CoordinateMapper;

        // get the depth (display) extents
        FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

        // open the reader for the body frames
        this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

        // set IsAvailableChanged event notifier
        this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

        // open the sensor
        this.kinectSensor.Open();

        if (this.bodyFrameReader != null) {
            this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
        }
    }

    /// <summary>
    /// Execute start up tasks
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void MainWindow_Loaded(object sender) {
        if (this.bodyFrameReader != null) {
            this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
        }
    }

    /// <summary>
    /// Execute shutdown tasks
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void MainWindow_Closing(object sender, CancelEventArgs e) {
        if (this.bodyFrameReader != null) {
            // BodyFrameReader is IDisposable
            this.bodyFrameReader.Dispose();
            this.bodyFrameReader = null;
        }

        if (this.kinectSensor != null) {
            this.kinectSensor.Close();
            this.kinectSensor = null;
        }
    }

    /// <summary>
    /// Handles the body frame data arriving from the sensor
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e) {
        bool dataReceived = false;

        using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame()) {
            if (bodyFrame != null) {
                if (this.bodies == null) {
                    this.bodies = new Body[bodyFrame.BodyCount];
                }

                // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                // As long as those body objects are not disposed and not set to null in the array,
                // those body objects will be re-used.
                bodyFrame.GetAndRefreshBodyData(this.bodies);
                dataReceived = true;
            }
        }

        if (dataReceived) {
            activeBodies = new List<Body>();
            for(int i = 0; i < this.bodies.Length; i++) {
                Body body = this.bodies[i];

                if (body.IsTracked) {
                    /*Dictionary<JointType, Windows.Kinect.Joint> joints = body.Joints;

                    foreach (JointType jointType in joints.Keys) {
                        // sometimes the depth(Z) of an inferred joint may show as negative
                        // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        CameraSpacePoint position = joints[jointType].Position;
                        if (position.Z < 0) {
                            position.Z = InferredZPositionClamp;
                        }
                    }*/
                    activeBodies.Add(body);
                    //Debug.Log(joints[JointType.HandLeft].Position.X + " " + joints[JointType.HandLeft].Position.Y + " " + joints[JointType.HandLeft].Position.Z);
                }
            }
        }
    }

    /// <summary>
    /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
        // on failure, set the status text
        //this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
        //                                                : Properties.Resources.SensorNotAvailableStatusText;
    }

    public void Close() {
        if (this.kinectSensor != null) {
            this.kinectSensor.Close();
            this.kinectSensor = null;
        }
        //System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
