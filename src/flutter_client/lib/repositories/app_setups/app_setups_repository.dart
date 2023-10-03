import 'dart:collection';

import 'package:flutter/material.dart';
import 'package:flutter_client/repositories/app_setups/app_setup.dart';
import 'package:uuid/uuid.dart';

class AppSetupsRepository extends ChangeNotifier {
  final List<AppSetup> _appSetups = List.empty();
  final idGenerator = const Uuid();

  List<AppSetup> get appSetups => UnmodifiableListView(_appSetups);

  AppSetup get(String id) {
    return _appSetups.firstWhere((x) => x.id == id);
  }

  void create({required String name, required String path, String? arguments}) {
    _appSetups.add(AppSetup(id: idGenerator.v1(), name: name, path: path, arguments: arguments));
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
