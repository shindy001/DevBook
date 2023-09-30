import 'package:flutter/material.dart';

abstract class DevBookSizes {
  /// Minimal application window size
  static const Size minWindowSize = Size(800, 600);

  /// Application window size on app start
  static const Size startWindowSize = Size(1024, 768);

  /// Toolbar height
  static const double titleBarHeight = 36;

  /// Width of a standard (expanded) menu
  static const double standardMenuWidth = 200;

  /// Width of a compact menu
  static const double compactMenuWidth = 55;

  /// Side menu item height
  static const double sideMenuItemHeight = 50;

  /// Side menu active item marker width
  static const double sideMenuActiveItemMarkerWidth = 5;
}
