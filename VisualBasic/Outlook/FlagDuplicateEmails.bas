Attribute VB_Name = "Modul1"
Sub FlagDuplicates()
    On Error GoTo ErrorHandler
  
    Dim dictionary As New Scripting.dictionary
    With Application.ActiveExplorer.currentFolder
        'Make sure wo only handle mailitems
        If .DefaultItemType <> olMailItem Then Exit Sub
        
        Dim i As Long
        Dim messageKey As String
        Dim currentItem As Outlook.mailItem
        
        For i = 1 To .Items.Count
            Set currentItem = .Items(i)
            
            messageKey = currentItem.SentOn & currentItem.Subject
        
            If dictionary.Exists(messageKey) Then
                currentItem.UnRead = True
                currentItem.Save
            Else
                dictionary.Add messageKey, Nothing
            End If
        Next i
    End With
    
    Set dictionary = Nothing
    MsgBox ("Finished")
    Exit Sub

ErrorHandler:
   Debug.Print Err.Number, currentItem.Subject
   Resume Next

End Sub
Function GetHeader(outlookMailItem As Outlook.mailItem, strFilter As String)
    Dim i As Integer
    Dim messageHeaders As Variant

    messageHeaders = Split(outlookMailItem.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x007D001E"), vbLf)
    For j = 1 To UBound(messageHeaders)
        If LCase(messageHeaders(j)) Like strFilter Then
           GetHeader = messageHeaders(j)
           Exit Function
        End If
    Next j
End Function

Sub FlagWrongAccount()
    On Error GoTo ErrorHandler
  
    With Application.ActiveExplorer.currentFolder
        'Make sure wo only handle mailitems
        If .DefaultItemType <> olMailItem Then Exit Sub
        
        Dim i As Long
        Dim currentItem As Outlook.mailItem
        Dim messageHeader As String
        
        For i = 1 To .Items.Count
            Set currentItem = .Items(i)
            messageHeader = currentItem.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x007D001E")
            
            If messageHeader Like "user@example.com" Then
                currentItem.UnRead = True
                currentItem.Save
            End If
        Next i
    End With
    
    MsgBox ("Finished")
    Exit Sub

ErrorHandler:
   Debug.Print Err.Number, currentItem.Subject
   Resume Next

End Sub
