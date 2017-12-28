open FsXaml

type MainWin = XAML<"MainWin.xaml">

open System
open Gjallarhorn.Bindable.Framework
open Gjallarhorn.Wpf
open System.Windows
open Gjallarhorn.Bindable
open Gjallarhorn

type Message = |Number of int

let rand = Random()
let mbindings _ source model = 
    let getNumber() = 
        let number = rand.Next() 
        printfn "RAND = %A" number //print a number twice
        number |> Some
    Bind.Explicit.oneWay source "Number" model
    [
        Bind.Explicit.createCommand "NewNumber" source
        |> Observable.map (fun _ -> getNumber())
        |> Observable.toMessage Number
    ]

let mcomponent : IComponent<int, obj, Message> = 
    Component.fromExplicit mbindings

let app nav = 
    let model = 42
    let update message _ =
        match message with
        |Number number -> number
    Framework.application model update mcomponent nav

[<STAThread>]
[<EntryPoint>]
let main argv = 
    let nav = Navigation.singleView Application MainWin
    let app = app nav.Navigate
    Framework.RunApplication (nav, app)
    1