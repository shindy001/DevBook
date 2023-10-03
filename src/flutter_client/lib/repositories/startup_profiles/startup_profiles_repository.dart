import 'dart:collection';

import 'package:flutter/material.dart';
import 'package:flutter_client/repositories/startup_profiles/startup_profile.dart';
import 'package:uuid/uuid.dart';

class StartupProfilesRepository extends ChangeNotifier {
  final List<StartupProfile> _startupProfiles = List.empty();
  final idGenerator = const Uuid();

  List<StartupProfile> get startupProfiles => UnmodifiableListView(_startupProfiles);

  StartupProfile get(String id) {
    return _startupProfiles.firstWhere((x) => x.id == id);
  }

  void create({required String name, List<String>? appSetupsIds}) {
    _startupProfiles.add(StartupProfile(id: idGenerator.v1(), name: name, appSetupsIds: appSetupsIds));
    notifyListeners();
  }

  void update({required String id, required String name, List<String>? appSetupsIds}) {
    var itemIndex = _startupProfiles.indexWhere((x) => x.id == id);
    _startupProfiles[itemIndex] = StartupProfile(id: id, name: name, appSetupsIds: appSetupsIds);
    notifyListeners();
  }

  void delete({required String id}) {
    _startupProfiles.removeWhere((x) => x.id == id);
    notifyListeners();
  }
}
