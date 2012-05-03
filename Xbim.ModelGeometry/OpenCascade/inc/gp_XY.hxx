// This file is generated by WOK (CPPExt).
// Please do not edit this file; modify original file instead.
// The copyright and license terms as defined for the original file apply to 
// this header file considered to be the "object code" form of the original source.

#ifndef _gp_XY_HeaderFile
#define _gp_XY_HeaderFile

#ifndef _Standard_HeaderFile
#include <Standard.hxx>
#endif
#ifndef _Standard_Macro_HeaderFile
#include <Standard_Macro.hxx>
#endif

#ifndef _Standard_Real_HeaderFile
#include <Standard_Real.hxx>
#endif
#ifndef _Standard_Storable_HeaderFile
#include <Standard_Storable.hxx>
#endif
#ifndef _Standard_Integer_HeaderFile
#include <Standard_Integer.hxx>
#endif
#ifndef _Standard_Boolean_HeaderFile
#include <Standard_Boolean.hxx>
#endif
#ifndef _Standard_PrimitiveTypes_HeaderFile
#include <Standard_PrimitiveTypes.hxx>
#endif
class Standard_ConstructionError;
class Standard_OutOfRange;
class gp_Mat2d;


Standard_EXPORT const Handle(Standard_Type)& STANDARD_TYPE(gp_XY);


//!  This class describes a cartesian coordinate entity in 2D <br>
//!  space {X,Y}. This class is non persistent. This entity used <br>
//!  for algebraic calculation. An XY can be transformed with a <br>
//!  Trsf2d or a  GTrsf2d from package gp. <br>
//! It is used in vectorial computations or for holding this type <br>
//! of information in data structures. <br>
class gp_XY  {

public:
  void* operator new(size_t,void* anAddress) 
  {
    return anAddress;
  }
  void* operator new(size_t size) 
  {
    return Standard::Allocate(size); 
  }
  void  operator delete(void *anAddress) 
  {
    if (anAddress) Standard::Free((Standard_Address&)anAddress); 
  }

  //! Creates XY object with zero coordinates (0,0). <br>
      gp_XY();
  //! a number pair defined by the XY coordinates <br>
      gp_XY(const Standard_Real X,const Standard_Real Y);
  
//!  modifies the coordinate of range Index <br>
//!  Index = 1 => X is modified <br>
//!  Index = 2 => Y is modified <br>
//!   Raises OutOfRange if Index != {1, 2}. <br>
        void SetCoord(const Standard_Integer Index,const Standard_Real Xi) ;
  //!  For this number pair, assigns <br>
//!   the values X and Y to its coordinates <br>
        void SetCoord(const Standard_Real X,const Standard_Real Y) ;
  //! Assigns the given value to the X coordinate of this number pair. <br>
        void SetX(const Standard_Real X) ;
  //! Assigns the given value to the Y  coordinate of this number pair. <br>
        void SetY(const Standard_Real Y) ;
  
//!  returns the coordinate of range Index : <br>
//!  Index = 1 => X is returned <br>
//!  Index = 2 => Y is returned <br>
//! Raises OutOfRange if Index != {1, 2}. <br>
        Standard_Real Coord(const Standard_Integer Index) const;
  //! For this number pair, returns its coordinates X and Y. <br>
        void Coord(Standard_Real& X,Standard_Real& Y) const;
  //! Returns the X coordinate of this number pair. <br>
        Standard_Real X() const;
  //! Returns the Y coordinate of this number pair. <br>
        Standard_Real Y() const;
  //! Computes Sqrt (X*X + Y*Y) where X and Y are the two coordinates of this number pair. <br>
        Standard_Real Modulus() const;
  //! Computes X*X + Y*Y where X and Y are the two coordinates of this number pair. <br>
        Standard_Real SquareModulus() const;
  
//!  Returns true if the coordinates of this number pair are <br>
//! equal to the respective coordinates of the number pair <br>
//! Other, within the specified tolerance Tolerance. I.e.: <br>
//!  abs(<me>.X() - Other.X()) <= Tolerance and <br>
//!  abs(<me>.Y() - Other.Y()) <= Tolerance and <br>//! computations <br>
  Standard_EXPORT     Standard_Boolean IsEqual(const gp_XY& Other,const Standard_Real Tolerance) const;
  //! Computes the sum of this number pair and number pair Other <br>
//! <me>.X() = <me>.X() + Other.X() <br>
//! <me>.Y() = <me>.Y() + Other.Y() <br>
        void Add(const gp_XY& Other) ;
      void operator +=(const gp_XY& Other) 
{
  Add(Other);
}
  //! Computes the sum of this number pair and number pair Other <br>
//! new.X() = <me>.X() + Other.X() <br>
//! new.Y() = <me>.Y() + Other.Y() <br>
        gp_XY Added(const gp_XY& Other) const;
      gp_XY operator +(const gp_XY& Other) const
{
  return Added(Other);
}
  
//!  Real D = <me>.X() * Other.Y() - <me>.Y() * Other.X() <br>
        Standard_Real Crossed(const gp_XY& Right) const;
      Standard_Real operator ^(const gp_XY& Right) const
{
  return Crossed(Right);
}
  
//!  computes the magnitude of the cross product between <me> and <br>
//!  Right. Returns || <me> ^ Right || <br>
        Standard_Real CrossMagnitude(const gp_XY& Right) const;
  
//!  computes the square magnitude of the cross product between <me> and <br>
//!  Right. Returns || <me> ^ Right ||**2 <br>
        Standard_Real CrossSquareMagnitude(const gp_XY& Right) const;
  //! divides <me> by a real. <br>
        void Divide(const Standard_Real Scalar) ;
      void operator /=(const Standard_Real Scalar) 
{
  Divide(Scalar);
}
  //! Divides <me> by a real. <br>
        gp_XY Divided(const Standard_Real Scalar) const;
      gp_XY operator /(const Standard_Real Scalar) const
{
  return Divided(Scalar);
}
  //! Computes the scalar product between <me> and Other <br>
        Standard_Real Dot(const gp_XY& Other) const;
      Standard_Real operator *(const gp_XY& Other) const
{
  return Dot(Other);
}
  
//!  <me>.X() = <me>.X() * Scalar; <br>
//!  <me>.Y() = <me>.Y() * Scalar; <br>
        void Multiply(const Standard_Real Scalar) ;
      void operator *=(const Standard_Real Scalar) 
{
  Multiply(Scalar);
}
  
//!  <me>.X() = <me>.X() * Other.X(); <br>
//!  <me>.Y() = <me>.Y() * Other.Y(); <br>
        void Multiply(const gp_XY& Other) ;
      void operator *=(const gp_XY& Other) 
{
  Multiply(Other);
}
  //! <me> = Matrix * <me> <br>
        void Multiply(const gp_Mat2d& Matrix) ;
      void operator *=(const gp_Mat2d& Matrix) 
{
  Multiply(Matrix);
}
  
//!  New.X() = <me>.X() * Scalar; <br>
//!  New.Y() = <me>.Y() * Scalar; <br>
        gp_XY Multiplied(const Standard_Real Scalar) const;
      gp_XY operator *(const Standard_Real Scalar) const
{
  return Multiplied(Scalar);
}
  
//!  new.X() = <me>.X() * Other.X(); <br>
//!  new.Y() = <me>.Y() * Other.Y(); <br>
        gp_XY Multiplied(const gp_XY& Other) const;
  //!  New = Matrix * <me> <br>
        gp_XY Multiplied(const gp_Mat2d& Matrix) const;
      gp_XY operator *(const gp_Mat2d& Matrix) const
{
  return Multiplied(Matrix);
}
  
//!  <me>.X() = <me>.X()/ <me>.Modulus() <br>
//!  <me>.Y() = <me>.Y()/ <me>.Modulus() <br>
//! Raises ConstructionError if <me>.Modulus() <= Resolution from gp <br>
        void Normalize() ;
  
//!  New.X() = <me>.X()/ <me>.Modulus() <br>
//!  New.Y() = <me>.Y()/ <me>.Modulus() <br>
//! Raises ConstructionError if <me>.Modulus() <= Resolution from gp <br>
        gp_XY Normalized() const;
  
//!  <me>.X() = -<me>.X() <br>
//!  <me>.Y() = -<me>.Y() <br>
        void Reverse() ;
  
//!  New.X() = -<me>.X() <br>
//!  New.Y() = -<me>.Y() <br>
        gp_XY Reversed() const;
      gp_XY operator -() const
{
  return Reversed();
}
  
//!  Computes  the following linear combination and <br>
//! assigns the result to this number pair: <br>
//!  A1 * XY1 + A2 * XY2 <br>
        void SetLinearForm(const Standard_Real A1,const gp_XY& XY1,const Standard_Real A2,const gp_XY& XY2) ;
  
//!  --  Computes  the following linear combination and <br>
//! assigns the result to this number pair: <br>
//!  A1 * XY1 + A2 * XY2 + XY3 <br>
        void SetLinearForm(const Standard_Real A1,const gp_XY& XY1,const Standard_Real A2,const gp_XY& XY2,const gp_XY& XY3) ;
  
//!  Computes  the following linear combination and <br>
//! assigns the result to this number pair: <br>
//!  A1 * XY1 + XY2 <br>
        void SetLinearForm(const Standard_Real A1,const gp_XY& XY1,const gp_XY& XY2) ;
  
//!   Computes  the following linear combination and <br>
//! assigns the result to this number pair: <br>
//!  XY1 + XY2 <br>
        void SetLinearForm(const gp_XY& XY1,const gp_XY& XY2) ;
  
//!  <me>.X() = <me>.X() - Other.X() <br>
//!  <me>.Y() = <me>.Y() - Other.Y() <br>
        void Subtract(const gp_XY& Right) ;
      void operator -=(const gp_XY& Right) 
{
  Subtract(Right);
}
  
//!  new.X() = <me>.X() - Other.X() <br>
//!  new.Y() = <me>.Y() - Other.Y() <br>
        gp_XY Subtracted(const gp_XY& Right) const;
      gp_XY operator -(const gp_XY& Right) const
{
  return Subtracted(Right);
}
    Standard_Real _CSFDB_Getgp_XYx() const { return x; }
    void _CSFDB_Setgp_XYx(const Standard_Real p) { x = p; }
    Standard_Real _CSFDB_Getgp_XYy() const { return y; }
    void _CSFDB_Setgp_XYy(const Standard_Real p) { y = p; }



protected:




private: 


Standard_Real x;
Standard_Real y;


};


#include <gp_XY.lxx>



// other Inline functions and methods (like "C++: function call" methods)


#endif
