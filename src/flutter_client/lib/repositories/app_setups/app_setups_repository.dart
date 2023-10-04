import 'dart:collection';

import 'package:flutter/material.dart';
import 'package:flutter_client/repositories/app_setups/app_setup.dart';
import 'package:uuid/uuid.dart';

class AppSetupsRepository extends ChangeNotifier {
  late final List<AppSetup> _appSetups = [
    AppSetup(id: idGenerator.v4(), name: "app1", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app2", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app3", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app4", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app5", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app6", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app9", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app7", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app8", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3'),
    AppSetup(id: idGenerator.v4(), name: "app10", path: 'c:/app/app.exe', arguments: 'arg1 arg2 arg3')
  ];
  final idGenerator = const Uuid();

  List<AppSetup> get appSetups => UnmodifiableListView(_appSetups);

  AppSetup get(String id) {
    return _appSetups.firstWhere((x) => x.id == id);
  }

  void create({required String name, required String path, String? arguments}) {
    _appSetups.add(AppSetup(id: idGenerator.v4(), name: name, path: path, arguments: arguments));
    notifyListeners();
  }

  void update({required String id, required String name, required String path, String? arguments}) {
    var itemIndex = _appSetups.indexWhere((x) => x.id == id);
    _appSetups[itemIndex] = AppSetup(id: id, name: name, path: path, arguments: arguments);
    notifyListeners();
  }

  void delete({required String id}) {
    _appSetups.removeWhere((x) => x.id == id);
    notifyListeners();
  }
}
