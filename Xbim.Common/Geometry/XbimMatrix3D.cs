﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xbim.Common.Exceptions;

namespace Xbim.Common.Geometry
{
    public struct XbimMatrix3D
    {
        #region members

        private static readonly XbimMatrix3D _identity;
        private Single _m11;
        private Single _m12;
        private Single _m13;
        private Single _m14;
        private Single _m21;
        private Single _m22;
        private Single _m23;
        private Single _m24;
        private Single _m31;
        private Single _m32;
        private Single _m33;
        private Single _m34;
        private Single _offsetX;
        private Single _offsetY;
        private Single _offsetZ;
        private Single _m44;
        private const Single FLOAT_EPSILON = 0.000001f;
        private bool _isNotDefaultInitialised;


        public Single M11
        {
            get
            {
                if(!_isNotDefaultInitialised) this = _identity;
                return _m11;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m11 = value;
            }
        }
        public Single M12
        {
            get
            {
                return _m12;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m12 = value;
            }
        }
        public Single M13
        {
            get
            {
                return _m13;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m13 = value;
            }
        }
        public Single M14
        {
            get
            {
                return _m14;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m14 = value;
            }
        }
        public Single M21
        {
            get
            {
                return _m21;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m21 = value;
            }
        }
        public Single M22
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _m22;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m22 = value;
            }
        }
        public Single M23
        {
            get
            {
                return _m23;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m23 = value;
            }
        }
        public Single M24
        {
            get
            {
                return _m24;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m24 = value;
            }
        }
        public Single M31
        {
            get
            {
                return _m31;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m31 = value;
            }
        }
        public Single M32
        {
            get
            {
                return _m32;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m32 = value;
            }
        }
        public Single M33
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _m33;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m33 = value;
            }
        }
        public Single M34
        {
            get
            {
                return _m34;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m34 = value;
            }
        }
        public Single OffsetX
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _offsetX;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._offsetX = value;
            }
        }
        public Single OffsetY
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _offsetY;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._offsetY = value;
            }
        }
        public Single OffsetZ
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _offsetZ;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._offsetZ = value;
            }
        }
        public Single M44
        {
            get
            {
                if (!_isNotDefaultInitialised) this = _identity;
                return _m44;
            }
            set
            {
                if (!_isNotDefaultInitialised) this = _identity;
                this._m44 = value;
            }
        }
       
        #endregion

        static XbimMatrix3D()
        {
            _identity = new XbimMatrix3D(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
            _identity._isNotDefaultInitialised = true;
        }

        public bool IsAffine
        {
            get
            {
                return (this._m14 == 0.0 && this._m24 == 0.0 && this._m34 == 0.0 && this._m44 == 1.0);
            }
        }

        public static XbimMatrix3D Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsIdentity 
        {
            get
            {
                if (!_isNotDefaultInitialised)
                {
                    this = _identity;
                    return true;
                }
                else return XbimMatrix3D.Equal(this, _identity);
            }
        }
      
        /// <summary>
        /// Creates a new instance of a XbimMatrix3D, initializing it with the given arguments
        /// </summary>
        public XbimMatrix3D(Single m11, Single m12, Single m13, Single m14, Single m21, Single m22, Single m23, Single m24, Single m31, Single m32, Single m33, Single m34, Single OffsetX, Single OffsetY, Single OffsetZ, Single m44)
        {    
            _m11 = m11;
            _m12 = m12;
            _m13 = m13;
            _m14 = m14;
            _m21 = m21;
            _m22 = m22;
            _m23 = m23;
            _m24 = m24;
            _m31 = m31;
            _m32 = m32;
            _m33 = m33;
            _m34 = m34;
            _offsetX = OffsetX;
            _offsetY = OffsetY;
            _offsetZ = OffsetZ;
            _m44 = m44;
             _isNotDefaultInitialised=true;
        }
        public XbimMatrix3D(XbimVector3D Offset)
        {
            _m11 = Identity.M11;
            _m12 = Identity.M12;
            _m13 = Identity.M13;
            _m14 = Identity.M14;
            _m21 = Identity.M21;
            _m22 = Identity.M22;
            _m23 = Identity.M23;
            _m24 = Identity.M24;
            _m31 = Identity.M31;
            _m32 = Identity.M32;
            _m33 = Identity.M33;
            _m34 = Identity.M34;
            _offsetX = Offset.X;
            _offsetY = Offset.Y;
            _offsetZ = Offset.Z;
            _m44 = Identity.M44;
            _isNotDefaultInitialised = true;
        }
        public XbimMatrix3D(Single Scale)
        {
            _m11 = Scale;
            _m12 = Identity.M12;
            _m13 = Identity.M13;
            _m14 = Identity.M14;
            _m21 = Identity.M21;
            _m22 = Scale;
            _m23 = Identity.M23;
            _m24 = Identity.M24;
            _m31 = Identity.M31;
            _m32 = Identity.M32;
            _m33 = Scale;
            _m34 = Identity.M34;
            _offsetX = Identity.OffsetX;
            _offsetY = Identity.OffsetY;
            _offsetZ = Identity.OffsetZ;
            _m44 = Identity.M44; ;
            _isNotDefaultInitialised = true;
        }
        /// <summary>
        /// Initialises with doubles, there is a possible loss of data as this matrix uses floats internally
        /// </summary>
        public XbimMatrix3D(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double OffsetX, double OffsetY, double OffsetZ, double m44)
        {
            _m11 = (float)m11;
            _m12 = (float)m12;
            _m13 = (float)m13;
            _m14 = (float)m14;
            _m21 = (float)m21;
            _m22 = (float)m22;
            _m23 = (float)m23;
            _m24 = (float)m24;
            _m31 = (float)m31;
            _m32 = (float)m32;
            _m33 = (float)m33;
            _m34 = (float)m34;
            _offsetX = (float)OffsetX;
            _offsetY = (float)OffsetY;
            _offsetZ = (float)OffsetZ;
            _m44 = (float)m44;
            _isNotDefaultInitialised = true;
        }
        /// <summary>
        /// Converts string of 15 reals to a matrix
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static XbimMatrix3D FromString(string val)
        {
            string[] itms = val.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (itms.Length == 1 && itms[0] == "I")
                return XbimMatrix3D.Identity;
            Debug.Assert(itms.Length == 16);
            return new XbimMatrix3D(
                Convert.ToSingle(itms[0], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[1], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[2], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[3], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[4], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[5], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[6], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[7], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[8], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[9], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[10], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[11], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[12], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[13], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[14], System.Globalization.CultureInfo.InvariantCulture),
                Convert.ToSingle(itms[15], System.Globalization.CultureInfo.InvariantCulture));   
        }

        public static XbimMatrix3D FromArray(byte[] array)
        {
            if (array.Length == 0) return XbimMatrix3D.Identity;
            MemoryStream ms = new MemoryStream(array);
            BinaryReader strm = new BinaryReader(ms);
            bool useDouble = array.Length > 16 * sizeof(Single);
            if (useDouble)
                return new XbimMatrix3D(
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble(),
                (float)strm.ReadDouble()
                );
            else
              return new XbimMatrix3D(
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle(),
                strm.ReadSingle()
                );
        }

        public XbimPoint3D Transform(XbimPoint3D p)
        {
            return XbimPoint3D.Multiply(p, this);
        }

        #region Operators


        public static XbimMatrix3D operator *(XbimMatrix3D a, XbimMatrix3D b)
        {
            return XbimMatrix3D.Multiply(a, b);
        }
        public override bool Equals(object obj)
        {
            if (obj is XbimMatrix3D )
            {

                return XbimMatrix3D.Equal(this, (XbimMatrix3D)obj);
            }
            else
                return false;
        }
        public override string ToString()
        {
            return this.Str();
        }
        public override int GetHashCode()
        {
            XbimPoint3D p = new XbimPoint3D(1, 3, 5); //get a point
            p = this.Transform(p); //tranform
            return p.GetHashCode(); //use the hash of the point
        }
        #endregion

        #region Accessors
        public XbimVector3D Up
        {
            get
            {
                return new XbimVector3D(_m21, _m22, _m23);
            }
        }
        public XbimVector3D Down
        {
            get
            {
                return new XbimVector3D(-_m21, -_m22,-_m23);
            }
        }
        public XbimVector3D Right
        {
            get
            {
                return new XbimVector3D(_m11, _m12, _m13);
            }
        }
        public XbimVector3D Left
        {
            get
            {
                return new XbimVector3D(-_m11, -_m12, -_m13);
            }
        }
        public XbimVector3D Forward
        {
            get
            {
                return new XbimVector3D(-_m31, -_m32, -_m33);
            }
        }
        public XbimVector3D Backward
        {
            get
            {
                return new XbimVector3D(_m31, _m32, _m33);
            }
        }

        public XbimVector3D Translation
        {
            get
            {
                return new XbimVector3D(_offsetX, _offsetY, _offsetZ);
            }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Performs a matrix multiplication
        /// </summary>
        /// <param name="mat">mat First operand</param>
        /// <param name="mat2">mat2 Second operand</param>
        /// <returns>dest if specified, mat otherwise</returns>
        public static XbimMatrix3D Multiply(XbimMatrix3D mat1, XbimMatrix3D mat2)
        {
            if (mat1.IsIdentity)
            {
                return mat2;
            }
            else if (mat2.IsIdentity)
            {
                return mat1;
            }
            else
            return new XbimMatrix3D((((mat1._m11 * mat2._m11) + (mat1._m12 * mat2._m21)) + (mat1._m13 * mat2._m31)) + (mat1._m14 * mat2._offsetX),
          (((mat1._m11 * mat2._m12) + (mat1._m12 * mat2._m22)) + (mat1._m13 * mat2._m32)) + (mat1._m14 * mat2._offsetY),
          (((mat1._m11 * mat2._m13) + (mat1._m12 * mat2._m23)) + (mat1._m13 * mat2._m33)) + (mat1._m14 * mat2._offsetZ),
          (((mat1._m11 * mat2._m14) + (mat1._m12 * mat2._m24)) + (mat1._m13 * mat2._m34)) + (mat1._m14 * mat2._m44),
          (((mat1._m21 * mat2._m11) + (mat1._m22 * mat2._m21)) + (mat1._m23 * mat2._m31)) + (mat1._m24 * mat2._offsetX),
          (((mat1._m21 * mat2._m12) + (mat1._m22 * mat2._m22)) + (mat1._m23 * mat2._m32)) + (mat1._m24 * mat2._offsetY),
          (((mat1._m21 * mat2._m13) + (mat1._m22 * mat2._m23)) + (mat1._m23 * mat2._m33)) + (mat1._m24 * mat2._offsetZ),
          (((mat1._m21 * mat2._m14) + (mat1._m22 * mat2._m24)) + (mat1._m23 * mat2._m34)) + (mat1._m24 * mat2._m44),
          (((mat1._m31 * mat2._m11) + (mat1._m32 * mat2._m21)) + (mat1._m33 * mat2._m31)) + (mat1._m34 * mat2._offsetX),
          (((mat1._m31 * mat2._m12) + (mat1._m32 * mat2._m22)) + (mat1._m33 * mat2._m32)) + (mat1._m34 * mat2._offsetY),
          (((mat1._m31 * mat2._m13) + (mat1._m32 * mat2._m23)) + (mat1._m33 * mat2._m33)) + (mat1._m34 * mat2._offsetZ),
          (((mat1._m31 * mat2._m14) + (mat1._m32 * mat2._m24)) + (mat1._m33 * mat2._m34)) + (mat1._m34 * mat2._m44),
          (((mat1._offsetX * mat2._m11) + (mat1._offsetY * mat2._m21)) + (mat1._offsetZ * mat2._m31)) + (mat1._m44 * mat2._offsetX),
          (((mat1._offsetX * mat2._m12) + (mat1._offsetY * mat2._m22)) + (mat1._offsetZ * mat2._m32)) + (mat1._m44 * mat2._offsetY),
          (((mat1._offsetX * mat2._m13) + (mat1._offsetY * mat2._m23)) + (mat1._offsetZ * mat2._m33)) + (mat1._m44 * mat2._offsetZ),
          (((mat1._offsetX * mat2._m14) + (mat1._offsetY * mat2._m24)) + (mat1._offsetZ * mat2._m34)) + (mat1._m44 * mat2._m44));

        }
        /// <summary>
        /// Compares two matrices for equality within a certain margin of error
        /// </summary>
        /// <param name="a">a First matrix</param>
        /// <param name="b">b Second matrix</param>
        /// <returns>True if a is equivalent to b</returns>
        public static bool Equal(XbimMatrix3D a, XbimMatrix3D b)
        {
            return   System.Object.ReferenceEquals(a, b) || (
                Math.Abs(a.M11 - b.M11) < FLOAT_EPSILON &&
                Math.Abs(a.M12 - b.M12) < FLOAT_EPSILON &&
                Math.Abs(a.M13 - b.M13) < FLOAT_EPSILON &&
                Math.Abs(a.M14 - b.M14) < FLOAT_EPSILON &&
                Math.Abs(a.M21 - b.M21) < FLOAT_EPSILON &&
                Math.Abs(a.M22 - b.M22) < FLOAT_EPSILON &&
                Math.Abs(a.M23 - b.M23) < FLOAT_EPSILON &&
                Math.Abs(a.M24 - b.M34) < FLOAT_EPSILON &&
                Math.Abs(a.M31 - b.M31) < FLOAT_EPSILON &&
                Math.Abs(a.M32 - b.M32) < FLOAT_EPSILON &&
                Math.Abs(a.M33 - b.M33) < FLOAT_EPSILON &&
                Math.Abs(a.M34 - b.M34) < FLOAT_EPSILON &&
                Math.Abs(a.OffsetX - b.OffsetX) < FLOAT_EPSILON &&
                Math.Abs(a.OffsetY - b.OffsetY) < FLOAT_EPSILON &&
                Math.Abs(a.OffsetZ - b.OffsetZ) < FLOAT_EPSILON &&
                Math.Abs(a.M44 - b.M44) < FLOAT_EPSILON
            );
        }

        /// <summary>
        /// Creates a new instance of a mat4
        /// </summary>
        /// <param name="mat">Single[16] containing values to initialize with</param>
        /// <returns>New mat4New mat4</returns>
        public static XbimMatrix3D Copy(XbimMatrix3D m)
        {
            return new XbimMatrix3D(m.M11 , m.M12 , m.M13, m.M14 ,
                  m.M21 , m.M22 , m.M23, m.M24 ,
                  m.M31 , m.M32 , m.M33, m.M34 ,
                  m.OffsetX , m.OffsetY , m.OffsetZ , m.M44);
        }


        /// <summary>
        /// Creates a 3D scaling matrix.
        /// </summary>
        /// <param name="UniformScale">>The scaling factor along all axis.</param>
        /// <returns>The new scaling matrix</returns>
        public static XbimMatrix3D CreateScale(float UniformScale)
        {
            return CreateScale(UniformScale, UniformScale, UniformScale);
        }

        /// <summary>
        /// Creates a 3D scaling matrix.
        /// </summary>
        /// <param name="ScaleX">>The scaling factor along the x-axis.</param>
        /// <param name="ScaleY">>The scaling factor along the y-axis.</param>
        /// <param name="ScaleZ">>The scaling factor along the z-axis.</param>
        /// <returns>The new scaling matrix</returns>
        public static XbimMatrix3D CreateScale(float ScaleX, float ScaleY, float ScaleZ)
        {
            return new XbimMatrix3D(
                ScaleX, 0.0, 0.0, 0.0,
                0.0, ScaleY, 0.0, 0.0,
                0.0, 0.0, ScaleZ, 0.0,
                0.0, 0.0, 0.0, 1.0
                );
        }

        public static XbimMatrix3D CreateWorld(XbimVector3D position, XbimVector3D forward, XbimVector3D up)
        {
            // prepare vectors
            forward.Normalize();
            XbimVector3D vector = forward * -1;
            XbimVector3D vector2 = XbimVector3D.CrossProduct(up, vector);
            vector2.Normalize();
            XbimVector3D vector3 = XbimVector3D.CrossProduct(vector, vector2);

            // prepare matrix
            XbimMatrix3D result = new XbimMatrix3D(
                vector2.X, vector2.Y, vector2.Z, 0.0,
                vector3.X, vector3.Y, vector3.Z, 0.0,
                vector.X, vector.Y, vector.Z, 0.0,
                position.X, position.Y, position.Z, 0.0);
            return result;
        }

        // Microsoft.Xna.Framework.Matrix
        public static XbimMatrix3D CreateLookAt(XbimVector3D cameraPosition, XbimVector3D cameraTarget, XbimVector3D cameraUpVector)
        {
            // prepare vectors
            XbimVector3D vector = cameraPosition - cameraTarget;
            vector.Normalize();
            XbimVector3D vector2 = XbimVector3D.CrossProduct(cameraUpVector, vector);
            vector2.Normalize();
            XbimVector3D vector3 = XbimVector3D.CrossProduct(vector, vector2);

            // prepare matrix
            XbimMatrix3D result = new XbimMatrix3D(
                vector2.X, vector3.X, vector.X, 0.0,
                vector2.Y, vector3.Y, vector.Y, 0.0,
                vector2.Z, vector3.Z, vector.Z, 0.0,
                -XbimVector3D.DotProduct(vector2, cameraPosition), -XbimVector3D.DotProduct(vector3, cameraPosition), -XbimVector3D.DotProduct(vector, cameraPosition), 1.0);
            return result;
        }


        /// <summary>
        /// Creates a 3D translation matrix.
        /// </summary>
        public static XbimMatrix3D CreateTranslation(float X, float Y, float Z)
        {
            return new XbimMatrix3D(
                1.0, 0.0, 0.0, 0.0,
                0.0, 1.0, 0.0, 0.0,
                0.0, 0.0, 1.0, 0.0,
                X, Y, Z, 1.0
                );
        }

        /// <summary>
        /// Creates a 3D translation matrix.
        /// </summary>
        public static XbimMatrix3D CreateTranslation(XbimVector3D TranslationVector)
        {
            return CreateTranslation(TranslationVector.X, TranslationVector.Y, TranslationVector.Z);
        }

        /// <summary>
        /// Returns a string representation of a mat4
        /// </summary>
        /// <param name="mat">mat mat4 to represent as a string</param>
        /// <returns>String representation of mat</returns>
        public  string Str()
        {
            if(this.IsIdentity)
                return "I";
            else
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
                    "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}", 
                    M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, OffsetX, OffsetY, OffsetZ, M44);

        }
        #endregion


        
        /// <summary>
        /// Decomposes a matrix into a scale, rotation, and translation.
        /// </summary>
        /// <param name="scale">When the method completes, contains the scaling component of the decomposed matrix.</param>
        /// <param name="rotation">When the method completes, contains the rtoation component of the decomposed matrix.</param>
        /// <param name="translation">When the method completes, contains the translation component of the decomposed matrix.</param>
        /// <remarks>
        /// This method is designed to decompose an SRT transformation matrix only.
        /// </remarks>
        public bool Decompose(out XbimVector3D scale, out XbimQuaternion rotation, out XbimVector3D translation)
        {
            //Source: Unknown
            // References: http://www.gamedev.net/community/forums/topic.asp?topic_id=441695
            // via https://code.google.com/p/sharpdx/source/browse/Source/SharpDX/Matrix.cs?r=9f9e209b1be04f06f294bc6d72b06055ad6abdcc

            //Get the translation.
            translation = new XbimVector3D();
            translation.X = this._offsetX;
            translation.Y = this._offsetY;
            translation.Z = this._offsetZ;

            //Scaling is the length of the rows.
            scale = new XbimVector3D();
            scale.X = (float)Math.Sqrt((M11 * M11) + (M12 * M12) + (M13 * M13));
            scale.Y = (float)Math.Sqrt((M21 * M21) + (M22 * M22) + (M23 * M23));
            scale.Z = (float)Math.Sqrt((M31 * M31) + (M32 * M32) + (M33 * M33));

            //If any of the scaling factors are zero, than the rotation matrix can not exist.
            // 

            double ZeroTolerance = 0.000003;
            if (Math.Abs(scale.X) < ZeroTolerance ||
                Math.Abs(scale.Y) < ZeroTolerance ||
                Math.Abs(scale.Z) < ZeroTolerance)
            {
                rotation = new XbimQuaternion(); // defaults to identity
                return false;
            }

            //The rotation is the left over matrix after dividing out the scaling.
            XbimMatrix3D rotationmatrix = new XbimMatrix3D();
            rotationmatrix.M11 = M11 / scale.X;
            rotationmatrix.M12 = M12 / scale.X;
            rotationmatrix.M13 = M13 / scale.X;

            rotationmatrix.M21 = M21 / scale.Y;
            rotationmatrix.M22 = M22 / scale.Y;
            rotationmatrix.M23 = M23 / scale.Y;

            rotationmatrix.M31 = M31 / scale.Z;
            rotationmatrix.M32 = M32 / scale.Z;
            rotationmatrix.M33 = M33 / scale.Z;

            rotationmatrix.M44 = 1;

            XbimQuaternion.RotationMatrix(ref rotationmatrix, out rotation);
            return true;
        }



        public XbimVector3D Transform(XbimVector3D xbimVector3D)
        {
            return XbimVector3D.Multiply(xbimVector3D, this);
        }

        public void Invert()
        {
            
            // Cache the matrix values (makes for huge speed increases!)
            Single a00 = M11, a01 = M12, a02 = M13, a03 = M14,
                a10 = M21, a11 = M22, a12 = M23, a13 = M24,
                a20 = M31, a21 = M32, a22 = M33, a23 = M34,
                a30 = OffsetX, a31 = OffsetY, a32 = OffsetZ, a33 = M44,

                b00 = a00 * a11 - a01 * a10,
                b01 = a00 * a12 - a02 * a10,
                b02 = a00 * a13 - a03 * a10,
                b03 = a01 * a12 - a02 * a11,
                b04 = a01 * a13 - a03 * a11,
                b05 = a02 * a13 - a03 * a12,
                b06 = a20 * a31 - a21 * a30,
                b07 = a20 * a32 - a22 * a30,
                b08 = a20 * a33 - a23 * a30,
                b09 = a21 * a32 - a22 * a31,
                b10 = a21 * a33 - a23 * a31,
                b11 = a22 * a33 - a23 * a32,
                d = (b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06),
                invDet;

            // Calculate the determinant
            if (d == 0) { throw new XbimException("Matrix does not have an inverse"); }
            invDet = 1 / d;

            M11 = (a11 * b11 - a12 * b10 + a13 * b09) * invDet;
            M12 = (-a01 * b11 + a02 * b10 - a03 * b09) * invDet;
            M13 = (a31 * b05 - a32 * b04 + a33 * b03) * invDet;
            M14= (-a21 * b05 + a22 * b04 - a23 * b03) * invDet;
            M21 = (-a10 * b11 + a12 * b08 - a13 * b07) * invDet;
            M22 = (a00 * b11 - a02 * b08 + a03 * b07) * invDet;
            M23 = (-a30 * b05 + a32 * b02 - a33 * b01) * invDet;
            M24 = (a20 * b05 - a22 * b02 + a23 * b01) * invDet;
            M31 = (a10 * b10 - a11 * b08 + a13 * b06) * invDet;
            M32 = (-a00 * b10 + a01 * b08 - a03 * b06) * invDet;
            M33 = (a30 * b04 - a31 * b02 + a33 * b00) * invDet;
            M34 = (-a20 * b04 + a21 * b02 - a23 * b00) * invDet;
            OffsetX = (-a10 * b09 + a11 * b07 - a12 * b06) * invDet;
            OffsetY = (a00 * b09 - a01 * b07 + a02 * b06) * invDet;
            OffsetZ = (-a30 * b03 + a31 * b01 - a32 * b00) * invDet;
            M44 = (a20 * b03 - a21 * b01 + a22 * b00) * invDet;
        }


        public byte[] ToArray(bool useDouble = true)
        {
            if (useDouble)
            {
                Byte[] b = new Byte[16 * sizeof(double)];
                MemoryStream ms = new MemoryStream(b);
                BinaryWriter strm = new BinaryWriter(ms);
                strm.Write((double)M11);
                strm.Write((double)M12);
                strm.Write((double)M13);
                strm.Write((double)M14);
                strm.Write((double)M21);
                strm.Write((double)M22);
                strm.Write((double)M23);
                strm.Write((double)M24);
                strm.Write((double)M31);
                strm.Write((double)M32);
                strm.Write((double)M33);
                strm.Write((double)M34);
                strm.Write((double)OffsetX);
                strm.Write((double)OffsetY);
                strm.Write((double)OffsetZ);
                strm.Write((double)M44);
                return b;
            }
            else
            {
                Byte[] b = new Byte[16 * sizeof(float)];
                MemoryStream ms = new MemoryStream(b);
                BinaryWriter strm = new BinaryWriter(ms);
                strm.Write(M11);
                strm.Write(M12);
                strm.Write(M13);
                strm.Write(M14);
                strm.Write(M21);
                strm.Write(M22);
                strm.Write(M23);
                strm.Write(M24);
                strm.Write(M31);
                strm.Write(M32);
                strm.Write(M33);
                strm.Write(M34);
                strm.Write(OffsetX);
                strm.Write(OffsetY);
                strm.Write(OffsetZ);
                strm.Write(M44);
                return b;
            }
        }
        public void Scale(double s)
        {
            Scale((float)s);
        }
        public void Scale(float s)
        {
            
            M11 *= s;
            M12 *= s;
            M13 *= s;
            M14 *= s;
            M21 *= s;
            M22 *= s;
            M23 *= s;
            M24 *= s;
            M31 *= s;
            M32 *= s;
            M33 *= s;
            M34 *= s;
        }

        public void Scale(XbimVector3D xbimVector3D)
        {
            var x = xbimVector3D.X;
            var y = xbimVector3D.Y;
            var z = xbimVector3D.Z;
            M11 *= x;
            M12 *= x;
            M13 *= x;
            M14 *= x;
            M21 *= y;
            M22 *= y;
            M23 *= y;
            M24 *= y;
            M31 *= z;
            M32 *= z;
            M33 *= z;
            M34 *= z;
        }

        /// <summary>
        /// Apply a X-Axis rotation to the matrix
        /// </summary>
        /// <param name="radAngle">Angle in radians</param>
        public void RotateAroundXAxis(double radAngle)
        {
            //get trig values
            double sinValue = Math.Sin(radAngle);
            double cosValue = Math.Cos(radAngle);

            //save values for matrix as it now stands
            float m21 = _m21;
            float m22 = _m22;
            float m23 = _m23;
            float m24 = _m24;
            float m31 = _m31;
            float m32 = _m32;
            float m33 = _m33;
            float m34 = _m34;

            //Amend value to suit X axis rotation

            //Perform X axis-specific matrix multiplication (right hand rule)
            //| 1    0               0               0  |
            //| 0    cos(radAngle)   sin(radAngle)   0  |
            //| 0    -sin(radAngle)  cos(radAngle)   0  |
            //| 0    0               0               1  |
            if (this.IsIdentity)
            {
                _m22 = (float)cosValue;
                _m23 = (float)sinValue;
                _m32 = (float)-sinValue;
                _m33 = (float)cosValue;
            }
            else
            {
                _m21 = (float)((m21 * cosValue) + (m31 * sinValue));
                _m22 = (float)((m22 * cosValue) + (m32 * sinValue));
                _m23 = (float)((m23 * cosValue) + (m33 * sinValue));
                _m24 = (float)((m24 * cosValue) + (m34 * sinValue));

                _m31 = (float)((m21 * -sinValue) + (m31 * cosValue));
                _m32 = (float)((m22 * -sinValue) + (m32 * cosValue));
                _m33 = (float)((m23 * -sinValue) + (m33 * cosValue));
                _m34 = (float)((m24 * -sinValue) + (m34 * cosValue));
            }

            //Perform X axis-specific matrix multiplication (left hand rule)
            //| 1    0               0               0  |
            //| 0    cos(radAngle)   -sin(radAngle)  0  |
            //| 0    sin(radAngle)   cos(radAngle)   0  |
            //| 0    0               0               1  |
            //if (this.IsIdentity)
            //{
            //    _m22 = (float)cosValue;
            //    _m23 = (float)-sinValue;
            //    _m32 = (float)sinValue;
            //    _m33 = (float)cosValue;
            //}
            //else
            //{
            //    _m21 = (float)((m21 * cosValue) + (m31 * -sinValue));
            //    _m22 = (float)((m22 * cosValue) + (m32 * -sinValue));
            //    _m23 = (float)((m23 * cosValue) + (m33 * -sinValue));
            //    _m24 = (float)((m24 * cosValue) + (m34 * -sinValue));

            //    _m31 = (float)((m21 * sinValue) + (m31 * cosValue));
            //    _m32 = (float)((m22 * sinValue) + (m32 * cosValue));
            //    _m33 = (float)((m23 * sinValue) + (m33 * cosValue));
            //    _m34 = (float)((m24 * sinValue) + (m34 * cosValue));
            //}
        }

        /// <summary>
        /// Apply a Y-Axis rotation to the matrix
        /// </summary>
        /// <param name="radAngle">Angle in radians</param>
        public void RotateAroundYAxis(double radAngle)
        {
            //get trig values
            double sinValue = Math.Sin(radAngle);
            double cosValue = Math.Cos(radAngle);

            //save values for matrix as it now stands
            float m11 = _m11;
            float m12 = _m12;
            float m13 = _m13;
            float m14 = _m14;
            float m31 = _m31;
            float m32 = _m32;
            float m33 = _m33;
            float m34 = _m34;

            //Amend value to suit Y axis rotation

            //Perform Y axis-specific matrix multiplication (right hand rule)
            //| cos(radAngle)   0   -sin(radAngle)  0  |
            //| 0               1   0               0  |
            //| sin(radAngle)   0   cos(radAngle)   0  |
            //| 0               0   0               1  |
            if (this.IsIdentity)
            {
                _m11 = (float)cosValue;
                _m13 = (float)-sinValue;
                _m31 = (float)sinValue;
                _m33 = (float)cosValue;
            }
            else
            {
                _m11 = (float)((m11 * cosValue) + (m31 * -sinValue));
                _m12 = (float)((m12 * cosValue) + (m32 * -sinValue));
                _m13 = (float)((m13 * cosValue) + (m33 * -sinValue));
                _m14 = (float)((m14 * cosValue) + (m34 * -sinValue));

                _m31 = (float)((m11 * sinValue) + (m31 * cosValue));
                _m32 = (float)((m12 * sinValue) + (m32 * cosValue));
                _m33 = (float)((m13 * sinValue) + (m33 * cosValue));
                _m34 = (float)((m14 * sinValue) + (m34 * cosValue));
            }

            //Perform Y axis-specific matrix multiplication (left hand rule)
            //| cos(radAngle)   0   -sin(radAngle)  0  |
            //| 0               1   0               0  |
            //| sin(radAngle)   0   cos(radAngle)   0  |
            //| 0               0   0               1  |
            //if (this.IsIdentity)
            //{
            //    _m11 = (float)cosValue;
            //    _m13 = (float)sinValue;
            //    _m31 = (float)-sinValue;
            //    _m33 = (float)cosValue;
            //}
            //else
            //{
            //    _m11 = (float)((m11 * cosValue) + (m31 * sinValue));
            //    _m12 = (float)((m12 * cosValue) + (m32 * sinValue));
            //    _m13 = (float)((m13 * cosValue) + (m33 * sinValue));
            //    _m14 = (float)((m14 * cosValue) + (m34 * sinValue));

            //    _m31 = (float)((m11 * -sinValue) + (m31 * cosValue));
            //    _m32 = (float)((m12 * -sinValue) + (m32 * cosValue));
            //    _m33 = (float)((m13 * -sinValue) + (m33 * cosValue));
            //    _m34 = (float)((m14 * -sinValue) + (m34 * cosValue));
            //}
        }

        /// <summary>
        /// Apply a Z-Axis rotation to the matrix
        /// </summary>
        /// <param name="radAngle">Angle in radians</param>
        public void RotateAroundZAxis(double radAngle)
        {
            //get trig values
            double sinValue = Math.Sin(radAngle);
            double cosValue = Math.Cos(radAngle);

            //save values for matrix as it now stands
            float m11 = _m11;
            float m12 = _m12;
            float m13 = _m13;
            float m14 = _m14;
            float m21 = _m21;
            float m22 = _m22;
            float m23 = _m23;
            float m24 = _m24;

            //Amend value to suit Z axis rotation

            //Perform Z axis-specific matrix multiplication (right hand rule)
            //| cos(radAngle)   sin(radAngle)  0    0  |
            //| -sin(radAngle)   cos(radAngle) 0    0  |
            //| 0               0              1    0  |
            //| 0               0              0    1  |
            if (this.IsIdentity)
            {
                _m11 = (float)cosValue;
                _m12 = (float)sinValue;
                _m21 = (float)-sinValue;
                _m22 = (float)cosValue;
            }
            else
            {
                _m11 = (float)((m11 * cosValue) + (m21 * sinValue));
                _m12 = (float)((m12 * cosValue) + (m22 * sinValue));
                _m13 = (float)((m13 * cosValue) + (m23 * sinValue));
                _m14 = (float)((m14 * cosValue) + (m24 * sinValue));

                _m21 = (float)((m11 * -sinValue) + (m21 * cosValue));
                _m22 = (float)((m12 * -sinValue) + (m22 * cosValue));
                _m23 = (float)((m13 * -sinValue) + (m23 * cosValue));
                _m24 = (float)((m14 * -sinValue) + (m24 * cosValue));
            }

            //Perform Z axis-specific matrix multiplication (left hand rule)
            //| cos(radAngle)   -sin(radAngle) 0    0  |
            //| sin(radAngle)   cos(radAngle)  0    0  |
            //| 0               0              1    0  |
            //| 0               0              0    1  |
            //if (this.IsIdentity)
            //{
            //    _m11 = (float)cosValue;
            //    _m12 = (float)-sinValue;
            //    _m21 = (float)sinValue;
            //    _m22 = (float)cosValue;
            //}
            //else
            //{
            //    _m11 = (float)((m11 * cosValue) + (m21 * -sinValue));
            //    _m12 = (float)((m12 * cosValue) + (m22 * -sinValue));
            //    _m13 = (float)((m13 * cosValue) + (m23 * -sinValue));
            //    _m14 = (float)((m14 * cosValue) + (m24 * -sinValue));

            //    _m21 = (float)((m11 * sinValue) + (m21 * cosValue));
            //    _m22 = (float)((m12 * sinValue) + (m22 * cosValue));
            //    _m23 = (float)((m13 * sinValue) + (m23 * cosValue));
            //    _m24 = (float)((m14 * sinValue) + (m24 * cosValue));
            //}
        }

        public XbimQuaternion GetRotationQuaternion()
        {
            XbimVector3D scale;
            XbimVector3D translation;
            XbimQuaternion rotation;
            this.Decompose(out scale, out rotation, out translation);
            return rotation;
        }
    }
}
