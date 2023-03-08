namespace LibGDXSharp.Graphics
{
    public interface IGL20
    {
        public const int GL_Es_Version_2_0                               = 1;
        public const int GL_Depth_Buffer_Bit                             = 0x00000100;
        public const int GL_Stencil_Buffer_Bit                           = 0x00000400;
        public const int GL_Color_Buffer_Bit                             = 0x00004000;
        public const int GL_False                                        = 0;
        public const int GL_True                                         = 1;
        public const int GL_Points                                       = 0x0000;
        public const int GL_Lines                                        = 0x0001;
        public const int GL_Line_Loop                                    = 0x0002;
        public const int GL_Line_Strip                                   = 0x0003;
        public const int GL_Triangles                                    = 0x0004;
        public const int GL_Triangle_Strip                               = 0x0005;
        public const int GL_Triangle_Fan                                 = 0x0006;
        public const int GL_Zero                                         = 0;
        public const int GL_One                                          = 1;
        public const int GL_Src_Color                                    = 0x0300;
        public const int GL_One_Minus_Src_Color                          = 0x0301;
        public const int GL_Src_Alpha                                    = 0x0302;
        public const int GL_One_Minus_Src_Alpha                          = 0x0303;
        public const int GL_Dst_Alpha                                    = 0x0304;
        public const int GL_One_Minus_Dst_Alpha                          = 0x0305;
        public const int GL_Dst_Color                                    = 0x0306;
        public const int GL_One_Minus_Dst_Color                          = 0x0307;
        public const int GL_Src_Alpha_Saturate                           = 0x0308;
        public const int GL_Func_Add                                     = 0x8006;
        public const int GL_Blend_Equation                               = 0x8009;
        public const int GL_Blend_Equation_Rgb                           = 0x8009;
        public const int GL_Blend_Equation_Alpha                         = 0x883D;
        public const int GL_Func_Subtract                                = 0x800A;
        public const int GL_Func_Reverse_Subtract                        = 0x800B;
        public const int GL_Blend_Dst_Rgb                                = 0x80C8;
        public const int GL_Blend_Src_Rgb                                = 0x80C9;
        public const int GL_Blend_Dst_Alpha                              = 0x80CA;
        public const int GL_Blend_Src_Alpha                              = 0x80CB;
        public const int GL_Constant_Color                               = 0x8001;
        public const int GL_One_Minus_Constant_Color                     = 0x8002;
        public const int GL_Constant_Alpha                               = 0x8003;
        public const int GL_One_Minus_Constant_Alpha                     = 0x8004;
        public const int GL_Blend_Color                                  = 0x8005;
        public const int GL_Array_Buffer                                 = 0x8892;
        public const int GL_Element_Array_Buffer                         = 0x8893;
        public const int GL_Array_Buffer_Binding                         = 0x8894;
        public const int GL_Element_Array_Buffer_Binding                 = 0x8895;
        public const int GL_Stream_Draw                                  = 0x88E0;
        public const int GL_Static_Draw                                  = 0x88E4;
        public const int GL_Dynamic_Draw                                 = 0x88E8;
        public const int GL_Buffer_Size                                  = 0x8764;
        public const int GL_Buffer_Usage                                 = 0x8765;
        public const int GL_Current_Vertex_Attrib                        = 0x8626;
        public const int GL_Front                                        = 0x0404;
        public const int GL_Back                                         = 0x0405;
        public const int GL_Front_And_Back                               = 0x0408;
        public const int GL_Texture_2D                                   = 0x0DE1;
        public const int GL_Cull_Face                                    = 0x0B44;
        public const int GL_Blend                                        = 0x0BE2;
        public const int GL_Dither                                       = 0x0BD0;
        public const int GL_Stencil_Test                                 = 0x0B90;
        public const int GL_Depth_Test                                   = 0x0B71;
        public const int GL_Scissor_Test                                 = 0x0C11;
        public const int GL_Polygon_Offset_Fill                          = 0x8037;
        public const int GL_Sample_Alpha_To_Coverage                     = 0x809E;
        public const int GL_Sample_Coverage                              = 0x80A0;
        public const int GL_No_Error                                     = 0;
        public const int GL_Invalid_Enum                                 = 0x0500;
        public const int GL_Invalid_Value                                = 0x0501;
        public const int GL_Invalid_Operation                            = 0x0502;
        public const int GL_Out_Of_Memory                                = 0x0505;
        public const int GL_Cw                                           = 0x0900;
        public const int GL_Ccw                                          = 0x0901;
        public const int GL_Line_Width                                   = 0x0B21;
        public const int GL_Aliased_Point_Size_Range                     = 0x846D;
        public const int GL_Aliased_Line_Width_Range                     = 0x846E;
        public const int GL_Cull_Face_Mode                               = 0x0B45;
        public const int GL_Front_Face                                   = 0x0B46;
        public const int GL_Depth_Range                                  = 0x0B70;
        public const int GL_Depth_Writemask                              = 0x0B72;
        public const int GL_Depth_Clear_Value                            = 0x0B73;
        public const int GL_Depth_Func                                   = 0x0B74;
        public const int GL_Stencil_Clear_Value                          = 0x0B91;
        public const int GL_Stencil_Func                                 = 0x0B92;
        public const int GL_Stencil_Fail                                 = 0x0B94;
        public const int GL_Stencil_Pass_Depth_Fail                      = 0x0B95;
        public const int GL_Stencil_Pass_Depth_Pass                      = 0x0B96;
        public const int GL_Stencil_Ref                                  = 0x0B97;
        public const int GL_Stencil_Value_Mask                           = 0x0B93;
        public const int GL_Stencil_Writemask                            = 0x0B98;
        public const int GL_Stencil_Back_Func                            = 0x8800;
        public const int GL_Stencil_Back_Fail                            = 0x8801;
        public const int GL_Stencil_Back_Pass_Depth_Fail                 = 0x8802;
        public const int GL_Stencil_Back_Pass_Depth_Pass                 = 0x8803;
        public const int GL_Stencil_Back_Ref                             = 0x8CA3;
        public const int GL_Stencil_Back_Value_Mask                      = 0x8CA4;
        public const int GL_Stencil_Back_Writemask                       = 0x8CA5;
        public const int GL_Viewport                                     = 0x0BA2;
        public const int GL_Scissor_Box                                  = 0x0C10;
        public const int GL_Color_Clear_Value                            = 0x0C22;
        public const int GL_Color_Writemask                              = 0x0C23;
        public const int GL_Unpack_Alignment                             = 0x0CF5;
        public const int GL_Pack_Alignment                               = 0x0D05;
        public const int GL_Max_Texture_Size                             = 0x0D33;
        public const int GL_Max_Texture_Units                            = 0x84E2;
        public const int GL_Max_Viewport_Dims                            = 0x0D3A;
        public const int GL_Subpixel_Bits                                = 0x0D50;
        public const int GL_Red_Bits                                     = 0x0D52;
        public const int GL_Green_Bits                                   = 0x0D53;
        public const int GL_Blue_Bits                                    = 0x0D54;
        public const int GL_Alpha_Bits                                   = 0x0D55;
        public const int GL_Depth_Bits                                   = 0x0D56;
        public const int GL_Stencil_Bits                                 = 0x0D57;
        public const int GL_Polygon_Offset_Units                         = 0x2A00;
        public const int GL_Polygon_Offset_Factor                        = 0x8038;
        public const int GL_Texture_Binding_2D                           = 0x8069;
        public const int GL_Sample_Buffers                               = 0x80A8;
        public const int GL_Samples                                      = 0x80A9;
        public const int GL_Sample_Coverage_Value                        = 0x80AA;
        public const int GL_Sample_Coverage_Invert                       = 0x80AB;
        public const int GL_Num_Compressed_Texture_Formats               = 0x86A2;
        public const int GL_Compressed_Texture_Formats                   = 0x86A3;
        public const int GL_Dont_Care                                    = 0x1100;
        public const int GL_Fastest                                      = 0x1101;
        public const int GL_Nicest                                       = 0x1102;
        public const int GL_Generate_Mipmap                              = 0x8191;
        public const int GL_Generate_Mipmap_Hint                         = 0x8192;
        public const int GL_Byte                                         = 0x1400;
        public const int GL_Unsigned_Byte                                = 0x1401;
        public const int GL_Short                                        = 0x1402;
        public const int GL_Unsigned_Short                               = 0x1403;
        public const int GL_Int                                          = 0x1404;
        public const int GL_Unsigned_Int                                 = 0x1405;
        public const int GL_Float                                        = 0x1406;
        public const int GL_Fixed                                        = 0x140C;
        public const int GL_Depth_Component                              = 0x1902;
        public const int GL_Alpha                                        = 0x1906;
        public const int GL_Rgb                                          = 0x1907;
        public const int GL_Rgba                                         = 0x1908;
        public const int GL_Luminance                                    = 0x1909;
        public const int GL_Luminance_Alpha                              = 0x190A;
        public const int GL_Unsigned_Short_4_4_4_4                       = 0x8033;
        public const int GL_Unsigned_Short_5_5_5_1                       = 0x8034;
        public const int GL_Unsigned_Short_5_6_5                         = 0x8363;
        public const int GL_Fragment_Shader                              = 0x8B30;
        public const int GL_Vertex_Shader                                = 0x8B31;
        public const int GL_Max_Vertex_Attribs                           = 0x8869;
        public const int GL_Max_Vertex_Uniform_Vectors                   = 0x8DFB;
        public const int GL_Max_Varying_Vectors                          = 0x8DFC;
        public const int GL_Max_Combined_Texture_Image_Units             = 0x8B4D;
        public const int GL_Max_Vertex_Texture_Image_Units               = 0x8B4C;
        public const int GL_Max_Texture_Image_Units                      = 0x8872;
        public const int GL_Max_Fragment_Uniform_Vectors                 = 0x8DFD;
        public const int GL_Shader_Type                                  = 0x8B4F;
        public const int GL_Delete_Status                                = 0x8B80;
        public const int GL_Link_Status                                  = 0x8B82;
        public const int GL_Validate_Status                              = 0x8B83;
        public const int GL_Attached_Shaders                             = 0x8B85;
        public const int GL_Active_Uniforms                              = 0x8B86;
        public const int GL_Active_Uniform_Max_Length                    = 0x8B87;
        public const int GL_Active_Attributes                            = 0x8B89;
        public const int GL_Active_Attribute_Max_Length                  = 0x8B8A;
        public const int GL_Shading_Language_Version                     = 0x8B8C;
        public const int GL_Current_Program                              = 0x8B8D;
        public const int GL_Never                                        = 0x0200;
        public const int GL_Less                                         = 0x0201;
        public const int GL_Equal                                        = 0x0202;
        public const int GL_Lequal                                       = 0x0203;
        public const int GL_Greater                                      = 0x0204;
        public const int GL_Notequal                                     = 0x0205;
        public const int GL_Gequal                                       = 0x0206;
        public const int GL_Always                                       = 0x0207;
        public const int GL_Keep                                         = 0x1E00;
        public const int GL_Replace                                      = 0x1E01;
        public const int GL_Incr                                         = 0x1E02;
        public const int GL_Decr                                         = 0x1E03;
        public const int GL_Invert                                       = 0x150A;
        public const int GL_Incr_Wrap                                    = 0x8507;
        public const int GL_Decr_Wrap                                    = 0x8508;
        public const int GL_Vendor                                       = 0x1F00;
        public const int GL_Renderer                                     = 0x1F01;
        public const int GL_Version                                      = 0x1F02;
        public const int GL_Extensions                                   = 0x1F03;
        public const int GL_Nearest                                      = 0x2600;
        public const int GL_Linear                                       = 0x2601;
        public const int GL_Nearest_Mipmap_Nearest                       = 0x2700;
        public const int GL_Linear_Mipmap_Nearest                        = 0x2701;
        public const int GL_Nearest_Mipmap_Linear                        = 0x2702;
        public const int GL_Linear_Mipmap_Linear                         = 0x2703;
        public const int GL_Texture_Mag_Filter                           = 0x2800;
        public const int GL_Texture_Min_Filter                           = 0x2801;
        public const int GL_Texture_Wrap_S                               = 0x2802;
        public const int GL_Texture_Wrap_T                               = 0x2803;
        public const int GL_Texture                                      = 0x1702;
        public const int GL_Texture_Cube_Map                             = 0x8513;
        public const int GL_Texture_Binding_Cube_Map                     = 0x8514;
        public const int GL_Texture_Cube_Map_Positive_X                  = 0x8515;
        public const int GL_Texture_Cube_Map_Negative_X                  = 0x8516;
        public const int GL_Texture_Cube_Map_Positive_Y                  = 0x8517;
        public const int GL_Texture_Cube_Map_Negative_Y                  = 0x8518;
        public const int GL_Texture_Cube_Map_Positive_Z                  = 0x8519;
        public const int GL_Texture_Cube_Map_Negative_Z                  = 0x851A;
        public const int GL_Max_Cube_Map_Texture_Size                    = 0x851C;
        public const int GL_Texture0                                     = 0x84C0;
        public const int GL_Texture1                                     = 0x84C1;
        public const int GL_Texture2                                     = 0x84C2;
        public const int GL_Texture3                                     = 0x84C3;
        public const int GL_Texture4                                     = 0x84C4;
        public const int GL_Texture5                                     = 0x84C5;
        public const int GL_Texture6                                     = 0x84C6;
        public const int GL_Texture7                                     = 0x84C7;
        public const int GL_Texture8                                     = 0x84C8;
        public const int GL_Texture9                                     = 0x84C9;
        public const int GL_Texture10                                    = 0x84CA;
        public const int GL_Texture11                                    = 0x84CB;
        public const int GL_Texture12                                    = 0x84CC;
        public const int GL_Texture13                                    = 0x84CD;
        public const int GL_Texture14                                    = 0x84CE;
        public const int GL_Texture15                                    = 0x84CF;
        public const int GL_Texture16                                    = 0x84D0;
        public const int GL_Texture17                                    = 0x84D1;
        public const int GL_Texture18                                    = 0x84D2;
        public const int GL_Texture19                                    = 0x84D3;
        public const int GL_Texture20                                    = 0x84D4;
        public const int GL_Texture21                                    = 0x84D5;
        public const int GL_Texture22                                    = 0x84D6;
        public const int GL_Texture23                                    = 0x84D7;
        public const int GL_Texture24                                    = 0x84D8;
        public const int GL_Texture25                                    = 0x84D9;
        public const int GL_Texture26                                    = 0x84DA;
        public const int GL_Texture27                                    = 0x84DB;
        public const int GL_Texture28                                    = 0x84DC;
        public const int GL_Texture29                                    = 0x84DD;
        public const int GL_Texture30                                    = 0x84DE;
        public const int GL_Texture31                                    = 0x84DF;
        public const int GL_Active_Texture                               = 0x84E0;
        public const int GL_Repeat                                       = 0x2901;
        public const int GL_Clamp_To_Edge                                = 0x812F;
        public const int GL_Mirrored_Repeat                              = 0x8370;
        public const int GL_Float_Vec2                                   = 0x8B50;
        public const int GL_Float_Vec3                                   = 0x8B51;
        public const int GL_Float_Vec4                                   = 0x8B52;
        public const int GL_Int_Vec2                                     = 0x8B53;
        public const int GL_Int_Vec3                                     = 0x8B54;
        public const int GL_Int_Vec4                                     = 0x8B55;
        public const int GL_Bool                                         = 0x8B56;
        public const int GL_Bool_Vec2                                    = 0x8B57;
        public const int GL_Bool_Vec3                                    = 0x8B58;
        public const int GL_Bool_Vec4                                    = 0x8B59;
        public const int GL_Float_Mat2                                   = 0x8B5A;
        public const int GL_Float_Mat3                                   = 0x8B5B;
        public const int GL_Float_Mat4                                   = 0x8B5C;
        public const int GL_Sampler_2D                                   = 0x8B5E;
        public const int GL_Sampler_Cube                                 = 0x8B60;
        public const int GL_Vertex_Attrib_Array_Enabled                  = 0x8622;
        public const int GL_Vertex_Attrib_Array_Size                     = 0x8623;
        public const int GL_Vertex_Attrib_Array_Stride                   = 0x8624;
        public const int GL_Vertex_Attrib_Array_Type                     = 0x8625;
        public const int GL_Vertex_Attrib_Array_Normalized               = 0x886A;
        public const int GL_Vertex_Attrib_Array_Pointer                  = 0x8645;
        public const int GL_Vertex_Attrib_Array_Buffer_Binding           = 0x889F;
        public const int GL_Implementation_Color_Read_Type               = 0x8B9A;
        public const int GL_Implementation_Color_Read_Format             = 0x8B9B;
        public const int GL_Compile_Status                               = 0x8B81;
        public const int GL_Info_Log_Length                              = 0x8B84;
        public const int GL_Shader_Source_Length                         = 0x8B88;
        public const int GL_Shader_Compiler                              = 0x8DFA;
        public const int GL_Shader_Binary_Formats                        = 0x8DF8;
        public const int GL_Num_Shader_Binary_Formats                    = 0x8DF9;
        public const int GL_Low_Float                                    = 0x8DF0;
        public const int GL_Medium_Float                                 = 0x8DF1;
        public const int GL_High_Float                                   = 0x8DF2;
        public const int GL_Low_Int                                      = 0x8DF3;
        public const int GL_Medium_Int                                   = 0x8DF4;
        public const int GL_High_Int                                     = 0x8DF5;
        public const int GL_Framebuffer                                  = 0x8D40;
        public const int GL_Renderbuffer                                 = 0x8D41;
        public const int GL_Rgba4                                        = 0x8056;
        public const int GL_Rgb5_A1                                      = 0x8057;
        public const int GL_Rgb565                                       = 0x8D62;
        public const int GL_Depth_Component16                            = 0x81A5;
        public const int GL_Stencil_Index                                = 0x1901;
        public const int GL_Stencil_Index8                               = 0x8D48;
        public const int GL_Renderbuffer_Width                           = 0x8D42;
        public const int GL_Renderbuffer_Height                          = 0x8D43;
        public const int GL_Renderbuffer_Internal_Format                 = 0x8D44;
        public const int GL_Renderbuffer_Red_Size                        = 0x8D50;
        public const int GL_Renderbuffer_Green_Size                      = 0x8D51;
        public const int GL_Renderbuffer_Blue_Size                       = 0x8D52;
        public const int GL_Renderbuffer_Alpha_Size                      = 0x8D53;
        public const int GL_Renderbuffer_Depth_Size                      = 0x8D54;
        public const int GL_Renderbuffer_Stencil_Size                    = 0x8D55;
        public const int GL_Framebuffer_Attachment_Object_Type           = 0x8CD0;
        public const int GL_Framebuffer_Attachment_Object_Name           = 0x8CD1;
        public const int GL_Framebuffer_Attachment_Texture_Level         = 0x8CD2;
        public const int GL_Framebuffer_Attachment_Texture_Cube_Map_Face = 0x8CD3;
        public const int GL_Color_Attachment0                            = 0x8CE0;
        public const int GL_Depth_Attachment                             = 0x8D00;
        public const int GL_Stencil_Attachment                           = 0x8D20;
        public const int GL_None                                         = 0;
        public const int GL_Framebuffer_Complete                         = 0x8CD5;
        public const int GL_Framebuffer_Incomplete_Attachment            = 0x8CD6;
        public const int GL_Framebuffer_Incomplete_Missing_Attachment    = 0x8CD7;
        public const int GL_Framebuffer_Incomplete_Dimensions            = 0x8CD9;
        public const int GL_Framebuffer_Unsupported                      = 0x8CDD;
        public const int GL_Framebuffer_Binding                          = 0x8CA6;
        public const int GL_Renderbuffer_Binding                         = 0x8CA7;
        public const int GL_Max_Renderbuffer_Size                        = 0x84E8;
        public const int GL_Invalid_Framebuffer_Operation                = 0x0506;
        public const int GL_Vertex_Program_Point_Size                    = 0x8642;

        // Extensions
        public const int GL_Coverage_Buffer_Bit_Nv         = 0x8000;
        public const int GL_Texture_Max_Anisotropy_Ext     = 0x84FE;
        public const int GL_Max_Texture_Max_Anisotropy_Ext = 0x84FF;

        public void GLActiveTexture( int texture );

        public void GLBindTexture( int target, int texture );

        public void GLBlendFunc( int sfactor, int dfactor );

        public void GLClear( int mask );

        public void GLClearColor( float red, float green, float blue, float alpha );

        public void GLClearDepthf( float depth );

        public void GLClearStencil( int s );

        public void GLColorMask( bool red, bool green, bool blue, bool alpha );

        public void GLCompressedTexImage2D( int target,
                                            int level,
                                            int internalformat,
                                            int width,
                                            int height,
                                            int border,
                                            int imageSize,
                                            Buffer data );

        public void GLCompressedTexSubImage2D( int target,
                                               int level,
                                               int xoffset,
                                               int yoffset,
                                               int width,
                                               int height,
                                               int format,
                                               int imageSize,
                                               Buffer data );

        public void GLCopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border );

        public void GLCopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height );

        public void GLCullFace( int mode );

        public void GLDeleteTextures( int n, IntBuffer textures );

        public void GLDeleteTexture( int texture );

        public void GLDepthFunc( int func );

        public void GLDepthMask( bool flag );

        public void GLDepthRangef( float zNear, float zFar );

        public void GLDisable( int cap );

        public void GLDrawArrays( int mode, int first, int count );

        public void GLDrawElements( int mode, int count, int type, Buffer indices );

        public void GLEnable( int cap );

        public void GLFinish();

        public void GLFlush();

        public void GLFrontFace( int mode );

        public void GLGenTextures( int n, IntBuffer textures );

        public int GLGenTexture();

        public int GLGetError();

        public void GLGetIntegerv( int pname, IntBuffer params );

        public String GLGetString( int name );

        public void GLHint( int target, int mode );

        public void GLLineWidth( float width );

        public void GLPixelStorei( int pname, int param );

        public void GLPolygonOffset( float factor, float units );

        public void GLReadPixels( int x, int y, int width, int height, int format, int type, Buffer pixels );

        public void GLScissor( int x, int y, int width, int height );

        public void GLStencilFunc( int func, int ref, int Mask);

        public void GLStencilMask( int mask );

        public void GLStencilOp( int fail, int zfail, int zpass );

        public void GLTexImage2D( int target,
                                  int level,
                                  int internalformat,
                                  int width,
                                  int height,
                                  int border,
                                  int format,
                                  int type,
                                  Buffer pixels );

        public void GLTexParameterf( int target, int pname, float param );

        public void GLTexSubImage2D( int target,
                                     int level,
                                     int xoffset,
                                     int yoffset,
                                     int width,
                                     int height,
                                     int format,
                                     int type,
                                     Buffer pixels );

        public void GLViewport( int x, int y, int width, int height );

        public void GLAttachShader( int program, int shader );

        public void GLBindAttribLocation( int program, int index, String name );

        public void GLBindBuffer( int target, int buffer );

        public void GLBindFramebuffer( int target, int framebuffer );

        public void GLBindRenderbuffer( int target, int renderbuffer );

        public void GLBlendColor( float red, float green, float blue, float alpha );

        public void GLBlendEquation( int mode );

        public void GLBlendEquationSeparate( int modeRgb, int modeAlpha );

        public void GLBlendFuncSeparate( int srcRgb, int dstRgb, int srcAlpha, int dstAlpha );

        public void GLBufferData( int target, int size, Buffer data, int usage );

        public void GLBufferSubData( int target, int offset, int size, Buffer data );

        public int GLCheckFramebufferStatus( int target );

        public void GLCompileShader( int shader );

        public int GLCreateProgram();

        public int GLCreateShader( int type );

        public void GLDeleteBuffer( int buffer );

        public void GLDeleteBuffers( int n, IntBuffer buffers );

        public void GLDeleteFramebuffer( int framebuffer );

        public void GLDeleteFramebuffers( int n, IntBuffer framebuffers );

        public void GLDeleteProgram( int program );

        public void GLDeleteRenderbuffer( int renderbuffer );

        public void GLDeleteRenderbuffers( int n, IntBuffer renderbuffers );

        public void GLDeleteShader( int shader );

        public void GLDetachShader( int program, int shader );

        public void GLDisableVertexAttribArray( int index );

        public void GLDrawElements( int mode, int count, int type, int indices );

        public void GLEnableVertexAttribArray( int index );

        public void GLFramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, int renderbuffer );

        public void GLFramebufferTexture2D( int target, int attachment, int textarget, int texture, int level );

        public int GLGenBuffer();

        public void GLGenBuffers( int n, IntBuffer buffers );

        public void GLGenerateMipmap( int target );

        public int GLGenFramebuffer();

        public void GLGenFramebuffers( int n, IntBuffer framebuffers );

        public int GLGenRenderbuffer();

        public void GLGenRenderbuffers( int n, IntBuffer renderbuffers );

        // deviates
        public String GLGetActiveAttrib( int program, int index, IntBuffer size, IntBuffer type );

        // deviates
        public String GLGetActiveUniform( int program, int index, IntBuffer size, IntBuffer type );

        public void GLGetAttachedShaders( int program, int maxcount, Buffer count, IntBuffer shaders );

        public int GLGetAttribLocation( int program, String name );

        public void GLGetBooleanv( int pname, Buffer params );

        public void GLGetBufferParameteriv( int target, int pname, IntBuffer params );

        public void GLGetFloatv( int pname, FloatBuffer params );

        public void GLGetFramebufferAttachmentParameteriv( int target, int attachment, int pname, IntBuffer params );

        public void GLGetProgramiv( int program, int pname, IntBuffer params );

        // deviates
        public String GLGetProgramInfoLog( int program );

        public void GLGetRenderbufferParameteriv( int target, int pname, IntBuffer params );

        public void GLGetShaderiv( int shader, int pname, IntBuffer params );

        // deviates
        public String GLGetShaderInfoLog( int shader );

        public void GLGetShaderPrecisionFormat( int shadertype, int precisiontype, IntBuffer range, IntBuffer precision );

        public void GLGetTexParameterfv( int target, int pname, FloatBuffer params );

        public void GLGetTexParameteriv( int target, int pname, IntBuffer params );

        public void GLGetUniformfv( int program, int location, FloatBuffer params );

        public void GLGetUniformiv( int program, int location, IntBuffer params );

        public int GLGetUniformLocation( int program, String name );

        public void GLGetVertexAttribfv( int index, int pname, FloatBuffer params );

        public void GLGetVertexAttribiv( int index, int pname, IntBuffer params );

        public void GLGetVertexAttribPointerv( int index, int pname, Buffer pointer );

        public bool GLIsBuffer( int buffer );

        public bool GLIsEnabled( int cap );

        public bool GLIsFramebuffer( int framebuffer );

        public bool GLIsProgram( int program );

        public bool GLIsRenderbuffer( int renderbuffer );

        public bool GLIsShader( int shader );

        public bool GLIsTexture( int texture );

        public void GLLinkProgram( int program );

        public void GLReleaseShaderCompiler();

        public void GLRenderbufferStorage( int target, int internalformat, int width, int height );

        public void GLSampleCoverage( float value, bool invert );

        public void GLShaderBinary( int n, IntBuffer shaders, int binaryformat, Buffer binary, int length );

        // Deviates
        public void GLShaderSource( int shader, String string );

        public void GLStencilFuncSeparate( int face, int func, int ref, int Mask);

        public void GLStencilMaskSeparate( int face, int mask );

        public void GLStencilOpSeparate( int face, int fail, int zfail, int zpass );

        public void GLTexParameterfv( int target, int pname, FloatBuffer params );

        public void GLTexParameteri( int target, int pname, int param );

        public void GLTexParameteriv( int target, int pname, IntBuffer params );

        public void GLUniform1F( int location, float x );

        public void GLUniform1Fv( int location, int count, FloatBuffer v );

        public void GLUniform1Fv( int location, int count, float v[], int Offset);

        public void GLUniform1I( int location, int x );

        public void GLUniform1Iv( int location, int count, IntBuffer v );

        public void GLUniform1Iv( int location, int count, int v[], int Offset);

        public void GLUniform2F( int location, float x, float y );

        public void GLUniform2Fv( int location, int count, FloatBuffer v );

        public void GLUniform2Fv( int location, int count, float v[], int Offset);

        public void GLUniform2I( int location, int x, int y );

        public void GLUniform2Iv( int location, int count, IntBuffer v );

        public void GLUniform2Iv( int location, int count, int[] v, int offset );

        public void GLUniform3F( int location, float x, float y, float z );

        public void GLUniform3Fv( int location, int count, FloatBuffer v );

        public void GLUniform3Fv( int location, int count, float[] v, int offset );

        public void GLUniform3I( int location, int x, int y, int z );

        public void GLUniform3Iv( int location, int count, IntBuffer v );

        public void GLUniform3Iv( int location, int count, int v[], int Offset);

        public void GLUniform4F( int location, float x, float y, float z, float w );

        public void GLUniform4Fv( int location, int count, FloatBuffer v );

        public void GLUniform4Fv( int location, int count, float v[], int Offset);

        public void GLUniform4I( int location, int x, int y, int z, int w );

        public void GLUniform4Iv( int location, int count, IntBuffer v );

        public void GLUniform4Iv( int location, int count, int v[], int Offset);

        public void GLUniformMatrix2Fv( int location, int count, bool transpose, FloatBuffer value );

        public void GLUniformMatrix2Fv( int location, int count, bool transpose, float value[], int Offset);

        public void GLUniformMatrix3Fv( int location, int count, bool transpose, FloatBuffer value );

        public void GLUniformMatrix3Fv( int location, int count, bool transpose, float value[], int Offset);

        public void GLUniformMatrix4Fv( int location, int count, bool transpose, FloatBuffer value );

        public void GLUniformMatrix4Fv( int location, int count, bool transpose, float value[], int Offset);

        public void GLUseProgram( int program );

        public void GLValidateProgram( int program );

        public void GLVertexAttrib1F( int indx, float x );

        public void GLVertexAttrib1Fv( int indx, FloatBuffer values );

        public void GLVertexAttrib2F( int indx, float x, float y );

        public void GLVertexAttrib2Fv( int indx, FloatBuffer values );

        public void GLVertexAttrib3F( int indx, float x, float y, float z );

        public void GLVertexAttrib3Fv( int indx, FloatBuffer values );

        public void GLVertexAttrib4F( int indx, float x, float y, float z, float w );

        public void GLVertexAttrib4Fv( int indx, FloatBuffer values );

        /**
	 * In OpenGl core profiles (3.1+), passing a pointer to client memory is not valid.
	 * In 3.0 and later, use the other version of this function instead, pass a zero-based
	 * offset which references the buffer currently bound to GL_ARRAY_BUFFER.
	 */
        public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, Buffer ptr );

        public void GLVertexAttribPointer( int indx, int size, int type, bool normalized, int stride, int ptr );
    }
}
