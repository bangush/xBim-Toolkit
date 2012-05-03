// This file is generated by WOK (CPPExt).
// Please do not edit this file; modify original file instead.
// The copyright and license terms as defined for the original file apply to 
// this header file considered to be the "object code" form of the original source.

#ifndef _GC_MakeArcOfEllipse_HeaderFile
#define _GC_MakeArcOfEllipse_HeaderFile

#ifndef _Standard_HeaderFile
#include <Standard.hxx>
#endif
#ifndef _Standard_Macro_HeaderFile
#include <Standard_Macro.hxx>
#endif

#ifndef _Handle_Geom_TrimmedCurve_HeaderFile
#include <Handle_Geom_TrimmedCurve.hxx>
#endif
#ifndef _GC_Root_HeaderFile
#include <GC_Root.hxx>
#endif
#ifndef _Standard_Real_HeaderFile
#include <Standard_Real.hxx>
#endif
#ifndef _Standard_Boolean_HeaderFile
#include <Standard_Boolean.hxx>
#endif
class Geom_TrimmedCurve;
class StdFail_NotDone;
class gp_Elips;
class gp_Pnt;


//! Implements construction algorithms for an arc <br>
//! of ellipse in 3D space. The result is a Geom_TrimmedCurve curve. <br>
//! A MakeArcOfEllipse object provides a framework for: <br>
//! -   defining the construction of the arc of ellipse, <br>
//! -   implementing the construction algorithm, and <br>
//! -   consulting the results. In particular, the <br>
//!   Value function returns the constructed arc of ellipse. <br>
class GC_MakeArcOfEllipse  : public GC_Root {
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

  //! Constructs an arc of Ellipse (TrimmedCurve from Geom) from <br>
//!          a Ellipse between two parameters Alpha1 and Alpha2. <br>
  Standard_EXPORT   GC_MakeArcOfEllipse(const gp_Elips& Elips,const Standard_Real Alpha1,const Standard_Real Alpha2,const Standard_Boolean Sense);
  //! Constructs an arc of Ellipse (TrimmedCurve from Geom) from <br>
//!          a Ellipse between point <P> and the angle Alpha <br>
//!          given in radians. <br>
  Standard_EXPORT   GC_MakeArcOfEllipse(const gp_Elips& Elips,const gp_Pnt& P,const Standard_Real Alpha,const Standard_Boolean Sense);
  //! Constructs an arc of Ellipse (TrimmedCurve from Geom) from <br>
//!          a Ellipse between two points P1 and P2. <br>
//! The orientation of the arc of ellipse is: <br>
//! -   the sense of Elips if Sense is true, or <br>
//! -   the opposite sense if Sense is false. <br>
//! Notes: <br>
//! -   Alpha1, Alpha2 and Alpha are angle values, given in radians. <br>
//! -   IsDone always returns true. <br>
  Standard_EXPORT   GC_MakeArcOfEllipse(const gp_Elips& Elips,const gp_Pnt& P1,const gp_Pnt& P2,const Standard_Boolean Sense);
  //! Returns the constructed arc of ellipse. <br>
  Standard_EXPORT    const Handle_Geom_TrimmedCurve& Value() const;
  
  Standard_EXPORT    const Handle_Geom_TrimmedCurve& Operator() const;
Standard_EXPORT operator Handle_Geom_TrimmedCurve() const;





protected:





private:



Handle_Geom_TrimmedCurve TheArc;


};





// other Inline functions and methods (like "C++: function call" methods)


#endif
