Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

Module modALUGEN

    '*   ActiveLock
    '*   Copyright 1998-2002 Nelson Ferraz
    '*   Copyright 2003-2005 The ActiveLock Software Group (ASG)
    '*   All material is the property of the contributing authors.
    '*
    '*   Redistribution and use in source and binary forms, with or without
    '*   modification, are permitted provided that the following conditions are
    '*   met:
    '*
    '*     [o] Redistributions of source code must retain the above copyright
    '*         notice, this list of conditions and the following disclaimer.
    '*
    '*     [o] Redistributions in binary form must reproduce the above
    '*         copyright notice, this list of conditions and the following
    '*         disclaimer in the documentation and/or other materials provided
    '*         with the distribution.
    '*
    '*   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
    '*   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
    '*   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
    '*   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
    '*   OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
    '*   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
    '*   LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    '*   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
    '*   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    '*   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
    '*   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
    '*
    '*

    ''
    ' Module used by ALUGEN.
    '
    ' @author ialkan
    ' @version 3.0.0
    ' @date 20050802
    '
    '* ///////////////////////////////////////////////////////////////////////
    '  /                        MODULE TO DO LIST                            /
    '  ///////////////////////////////////////////////////////////////////////

    '* ///////////////////////////////////////////////////////////////////////
    '  /                        MODULE CHANGE LOG                            /
    '  ///////////////////////////////////////////////////////////////////////
    ' @history
    ' <pre>
    ' 08.15.03 - th2tran       - Created
    ' 05.13.05 - ialkan        - Added this module to ActiveLock3NET.dll commented out duplicate entries
    ' 08.02.05 - ialkan        - Tested the module under VB.NET and made it run
    '
    ' </pre>

    '  ///////////////////////////////////////////////////////////////////////
    '  /                MODULE CODE BEGINS BELOW THIS LINE                   /
    '  ///////////////////////////////////////////////////////////////////////

    ' RSA Key Structure
    ' @param bits   Key length in bits
    ' @param data   Key data
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> Structure RSAKey ' 36-byte structure
        Dim bits As Integer
        '<VBFixedArray(32)> Dim data() As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> Dim data() As Byte
        Public Sub Initialize()
            ReDim data(32)
        End Sub
    End Structure
    ''
    ' Generates an RSA key set
    ' @param ptrKey RSA key structure
    ' @param bits   key length in bits
    ' @param pfn    TBD
    ' @param pfnparam    TBD
    Public Declare Function rsa_generate Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal bits As Integer, ByVal pfn As Integer, ByVal pfnparam As Integer) As Integer
    ''
    ' Generates an RSA key set without showing any progress
    ' needed for the VB.NET build
    ' @param ptrKey RSA key structure
    ' @param bits   key length in bits
    Public Declare Function rsa_generate2 Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal bits As Long) As Long
    ''
    ' Returns the public key blob fro the specified key.
    ' @param ptrKey RSA key structure
    ' @param blob   [Output] Key bob to be returned
    ' @param blobLen    Length of the key blob, in bytes
    Public Declare Function rsa_public_key_blob Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal blob As String, ByRef blobLen As Integer) As Integer

    ''
    ' Returns the private key blob fro the specified key.
    ' @param ptrKey RSA key structure
    ' @param blob   [Output] Key bob to be returned
    ' @param blobLen    Length of the key blob, in bytes
    Public Declare Function rsa_private_key_blob Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal blob As String, ByRef blobLen As Integer) As Integer

    ''
    ' Creates a new RSAKey from the specified key blobs.
    ' @param pub_blob   Public key blob
    ' @param pub_len    Length of public key blob, in bytes
    ' @param priv_blob  Private key blob
    ' @param priv_len   Length of private key blob, in bytes
    ' @param ptrKey     [out] RSA key to be returned.
    Public Declare Function rsa_createkey Lib "ALCrypto3NET" (ByVal pub_blob As String, ByVal pub_len As Integer, ByVal priv_blob As String, ByVal priv_len As Integer, ByRef ptrKey As RSAKey) As Integer
    ''
    ' Release memory allocated by rsa_createkey() to store the key.
    ' @param ptrKey     RSA key
    Public Declare Function rsa_freekey Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey) As Integer

    ''
    ' Signs the data using the specified RSA private key.
    ' @param ptrKey Key to be used for signing
    ' @param data   Data to be signed
    ' @param dLen   Data length
    ' @param sig    [out] Signature
    ' @param sLen   Signature length
    Public Declare Function rsa_sign Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal data As String, ByVal dLen As Integer, ByVal sig As String, ByRef sLen As Integer) As Integer

    ''
    ' Verifies an RSA signature.
    ' @param ptrKey Key to be used for signing
    ' @param sig    [out] Signature
    ' @param sLen   Signature length
    ' @param data   Data with which to verify
    ' @param dLen   Data length
    Public Declare Function rsa_verifysig Lib "ALCrypto3NET" (ByRef ptrKey As RSAKey, ByVal sig As String, ByVal sLen As Integer, ByVal data As String, ByVal dLen As Integer) As Integer

    Public Structure PhaseType
        Dim exponential As Byte
        Dim startpoint As Byte
        Dim total As Byte
        Dim param As Byte
        Dim current As Byte
        Dim n As Byte ' if exponential */
        Dim Mult As Byte ' if linear */
    End Structure
    Public Const MAXPHASE As Integer = 5
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> Public Structure ProgressType
        Dim nphases As Integer
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=MAXPHASE - 1)> Dim phases() As PhaseType
        Dim total As Byte
        Dim divisor As Byte
        Dim range As Byte
        Dim hwndProgbar As Integer
        Public Sub Initialize()
            ReDim phases(MAXPHASE - 1)
        End Sub
    End Structure
    Public ActiveLock3AlugenGlobals_definst As New ActiveLock3NET.AlugenGlobals
    Public ActiveLock3Globals_definst As New ActiveLock3NET.Globals_Renamed
    Public Const LICENSES_FILE As String = "authorizations.txt"
    Public sortOptions As Object
    Structure storageType
        <VBFixedArray(10)> Dim D() As String
        Public Sub Initialize()
            ReDim D(10)
        End Sub
    End Structure
    Public storage() As storageType

End Module