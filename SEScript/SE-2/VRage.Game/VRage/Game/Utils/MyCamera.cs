using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Utils
{
	public class MyCamera : IMyCamera
	{
		public const float DefaultFarPlaneDistance = 20000f;

		public float NearPlaneDistance = 0.05f;

		public float FarPlaneDistance = 20000f;

		public float FieldOfView;

		public Vector3D PreviousPosition;

		public MyViewport Viewport;

		public MatrixD WorldMatrix = MatrixD.Identity;

		public MatrixD ViewMatrix = MatrixD.Identity;

		public MatrixD ProjectionMatrix = MatrixD.Identity;

		public MatrixD ProjectionMatrixFar = MatrixD.Identity;

		public MatrixD ViewProjectionMatrix = MatrixD.Identity;

		public MatrixD ViewProjectionMatrixFar = MatrixD.Identity;

		public BoundingBoxD BoundingBox;

		public BoundingSphereD BoundingSphere;

		public MyCameraZoomProperties Zoom;

		public BoundingFrustumD BoundingFrustum = new BoundingFrustumD(MatrixD.Identity);

		public BoundingFrustumD BoundingFrustumFar = new BoundingFrustumD(MatrixD.Identity);

		/// <summary>
		/// Member that shakes with the camera.
		/// Note: If we start to have more cameras in the scene, this should be changed to component, because not every camera needs it.
		///       But currently - we use just one camera, so it is a member.
		/// </summary>
		public readonly MyCameraShake CameraShake = new MyCameraShake();

		/// <summary>
		/// Member that implements camera spring.
		/// Note: If we start to have more cameras in the scene, this should be changed to component, because not every camera needs it.
		///       But currently - we use just one camera, so it is a member.
		/// </summary>
		public readonly MyCameraSpring CameraSpring = new MyCameraSpring();

		/// <summary>
		/// FOV spring, value that is applied on top of current FOV
		/// </summary>
		private float m_fovSpring;

		/// <summary>
		/// FOV spring dampening.
		/// </summary>
		private static float m_fovSpringDampening = 0.5f;

		public float FarFarPlaneDistance = 1000000f;

		public float AspectRatio
		{
			get;
			private set;
		}

		/// <summary>
		/// Current view matrix without translation part.
		/// </summary>
		public MatrixD ViewMatrixAtZero
		{
			get
			{
				MatrixD viewProjectionMatrix = ViewProjectionMatrix;
				viewProjectionMatrix.M14 = 0.0;
				viewProjectionMatrix.M24 = 0.0;
				viewProjectionMatrix.M34 = 0.0;
				viewProjectionMatrix.M41 = 0.0;
				viewProjectionMatrix.M42 = 0.0;
				viewProjectionMatrix.M43 = 0.0;
				viewProjectionMatrix.M44 = 1.0;
				return viewProjectionMatrix;
			}
		}

		/// <summary>
		/// Forward vector of camera world matrix ("ahead from camera")
		/// </summary>
		public Vector3 ForwardVector => WorldMatrix.Forward;

		/// <summary>
		/// Left vector of camera world matrix ("to the left from camera")
		/// </summary>
		public Vector3 LeftVector => WorldMatrix.Left;

		/// <summary>
		/// Up vector of camera world matrix ("up from camera")
		/// </summary>
		public Vector3 UpVector => WorldMatrix.Up;

		/// <summary>
		/// Field of view in degrees.
		/// </summary>
		public float FieldOfViewDegrees
		{
			get
			{
				return MathHelper.ToDegrees(FieldOfView);
			}
			set
			{
				FieldOfView = MathHelper.ToRadians(value);
			}
		}

		/// <summary>
		/// Gets current fov with considering if zoom is enabled. Also add current fov spring value.
		/// </summary>
		public float FovWithZoom => Zoom.GetFOV() + m_fovSpring;

		/// <summary>
		/// Get position of the camera.
		/// </summary>
		public Vector3D Position => WorldMatrix.Translation;

		float IMyCamera.FieldOfViewAngle => FieldOfViewDegrees;

		float IMyCamera.FieldOfViewAngleForNearObjects => FieldOfViewDegrees;

		float IMyCamera.FovWithZoom => FovWithZoom;

		float IMyCamera.FovWithZoomForNearObjects => FovWithZoom;

		Vector3D IMyCamera.Position => Position;

		Vector3D IMyCamera.PreviousPosition => PreviousPosition;

		Vector2 IMyCamera.ViewportOffset => new Vector2(Viewport.OffsetX, Viewport.OffsetY);

		Vector2 IMyCamera.ViewportSize => new Vector2(Viewport.Width, Viewport.Height);

		MatrixD IMyCamera.ViewMatrix => ViewMatrix;

		MatrixD IMyCamera.WorldMatrix => WorldMatrix;

		MatrixD IMyCamera.ProjectionMatrix => ProjectionMatrix;

		MatrixD IMyCamera.ProjectionMatrixForNearObjects => ProjectionMatrix;

		float IMyCamera.NearPlaneDistance => NearPlaneDistance;

		float IMyCamera.FarPlaneDistance => FarPlaneDistance;

		public MyCamera(float fieldOfView, MyViewport currentScreenViewport)
		{
			FieldOfView = fieldOfView;
			Zoom = new MyCameraZoomProperties(this);
			UpdateScreenSize(currentScreenViewport);
		}

		public void Update(float updateStepTime)
		{
			Zoom.Update(updateStepTime);
			Vector3 newCameraLocalOffset = Vector3.Zero;
			MatrixD rotationMatrix = ViewMatrix;
			if (CameraSpring.Enabled)
			{
				CameraSpring.Update(updateStepTime, out newCameraLocalOffset);
			}
			if (CameraShake.ShakeEnabled)
			{
				CameraShake.UpdateShake(updateStepTime, out Vector3 outPos, out Vector3 _);
				newCameraLocalOffset += outPos;
			}
			if (newCameraLocalOffset != Vector3.Zero)
			{
				Vector3D vector = newCameraLocalOffset;
				Vector3D.Rotate(ref vector, ref rotationMatrix, out Vector3D result);
				rotationMatrix.Translation += result;
				ViewMatrix = rotationMatrix;
			}
			UpdatePropertiesInternal(ViewMatrix);
			m_fovSpring *= m_fovSpringDampening;
		}

		public void UpdateScreenSize(MyViewport currentScreenViewport)
		{
			Viewport = currentScreenViewport;
			PreviousPosition = Vector3D.Zero;
			BoundingFrustum = new BoundingFrustumD(MatrixD.Identity);
			AspectRatio = Viewport.Width / Viewport.Height;
		}

		public void SetViewMatrix(MatrixD newViewMatrix)
		{
			PreviousPosition = Position;
			UpdatePropertiesInternal(newViewMatrix);
		}

		public void UploadViewMatrixToRender()
		{
			MyRenderProxy.SetCameraViewMatrix(ViewMatrix, ProjectionMatrix, ProjectionMatrixFar, Zoom.GetFOV() + m_fovSpring, Zoom.GetFOV() + m_fovSpring, NearPlaneDistance, FarPlaneDistance, FarFarPlaneDistance, Position);
		}

		private void UpdatePropertiesInternal(MatrixD newViewMatrix)
		{
			ViewMatrix = newViewMatrix;
			MatrixD.Invert(ref ViewMatrix, out WorldMatrix);
			ProjectionMatrix = MatrixD.CreatePerspectiveFieldOfView(FovWithZoom, AspectRatio, GetSafeNear(), FarPlaneDistance);
			ProjectionMatrixFar = MatrixD.CreatePerspectiveFieldOfView(FovWithZoom, AspectRatio, GetSafeNear(), FarFarPlaneDistance);
			ViewProjectionMatrix = ViewMatrix * ProjectionMatrix;
			ViewProjectionMatrixFar = ViewMatrix * ProjectionMatrixFar;
			UpdateBoundingFrustum();
		}

		private float GetSafeNear()
		{
			return Math.Min(4f, NearPlaneDistance);
		}

		private void UpdateBoundingFrustum()
		{
			BoundingFrustum.Matrix = ViewProjectionMatrix;
			BoundingFrustumFar.Matrix = ViewProjectionMatrixFar;
			BoundingBox = BoundingBoxD.CreateInvalid();
			BoundingBox.Include(ref BoundingFrustum);
			BoundingSphere = MyUtils.GetBoundingSphereFromBoundingBox(ref BoundingBox);
		}

		public bool IsInFrustum(ref BoundingBoxD boundingBox)
		{
			BoundingFrustum.Contains(ref boundingBox, out ContainmentType result);
			return result != ContainmentType.Disjoint;
		}

		public bool IsInFrustum(BoundingBoxD boundingBox)
		{
			return IsInFrustum(ref boundingBox);
		}

		public bool IsInFrustum(ref BoundingSphereD boundingSphere)
		{
			BoundingFrustum.Contains(ref boundingSphere, out ContainmentType result);
			return result != ContainmentType.Disjoint;
		}

		public double GetDistanceFromPoint(Vector3D position)
		{
			return Vector3D.Distance(Position, position);
		}

		/// <summary>
		/// Gets screen coordinates of 3d world pos in 0 - 1 distance where 1.0 is screen width(for X) or height(for Y).
		/// WARNING: Y is from bottom to top.
		/// </summary>
		/// <param name="worldPos">World position.</param>
		/// <returns>Screen coordinate in 0-1 distance.</returns>
		public Vector3D WorldToScreen(ref Vector3D worldPos)
		{
			return Vector3D.Transform(worldPos, ViewProjectionMatrix);
		}

		/// <summary>
		/// Gets world coordinates from screen position in 0 - 1 distance where 1.0 is screen width(for X) or height(for Y).
		/// WARNING: Y is from bottom to top.
		/// </summary>
		/// <param name="worldPos">Screen coordinate in 0-1 distance.</param>
		/// <returns>World position.</returns>
		public Vector3D ScreenToWorld(ref Vector3D screenPos)
		{
			MatrixD matrix = MatrixD.Invert(MatrixD.Multiply(MatrixD.Multiply(WorldMatrix, ViewMatrix), ProjectionMatrix));
			return Vector3D.Transform(screenPos, matrix);
		}

		/// <summary>
		/// Gets normalized world space line from screen space coordinates.
		/// </summary>
		/// <param name="screenCoords"></param>
		/// <returns></returns>
		public LineD WorldLineFromScreen(Vector2 screenCoords)
		{
			MatrixD matrix = MatrixD.Invert(ViewProjectionMatrix);
			Vector4D vector = new Vector4D(2f * screenCoords.X / Viewport.Width - 1f, 1f - 2f * screenCoords.Y / Viewport.Height, 0.0, 1.0);
			Vector4D vector2 = new Vector4D(2f * screenCoords.X / Viewport.Width - 1f, 1f - 2f * screenCoords.Y / Viewport.Height, 1.0, 1.0);
			Vector4D xyz = Vector4D.Transform(vector, matrix);
			Vector4D xyz2 = Vector4D.Transform(vector2, matrix);
			xyz /= xyz.W;
			xyz2 /= xyz2.W;
			return new LineD(new Vector3D(xyz), new Vector3D(xyz2));
		}

		public void AddFovSpring(float fovAddition = 0.01f)
		{
			m_fovSpring += fovAddition;
		}

		Vector3D IMyCamera.WorldToScreen(ref Vector3D worldPos)
		{
			return Vector3D.Transform(worldPos, ViewProjectionMatrix);
		}

		double IMyCamera.GetDistanceWithFOV(Vector3D position)
		{
			return GetDistanceFromPoint(position);
		}

		bool IMyCamera.IsInFrustum(ref BoundingBoxD boundingBox)
		{
			return IsInFrustum(ref boundingBox);
		}

		bool IMyCamera.IsInFrustum(ref BoundingSphereD boundingSphere)
		{
			return IsInFrustum(ref boundingSphere);
		}

		bool IMyCamera.IsInFrustum(BoundingBoxD boundingBox)
		{
			return IsInFrustum(boundingBox);
		}
	}
}
