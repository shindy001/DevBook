/// Spacing used in the DevBook.
abstract class DevBookSpacing {
  /// The default unit of spacing
  static const double _spaceUnit = 16;

  /// xxxs spacing value (1pt)
  static const double xxxs = 0.0625 * _spaceUnit;

  /// xxs spacing value (2pt)
  static const double xxs = 0.125 * _spaceUnit;

  /// xs spacing value (4pt)
  static const double xs = 0.25 * _spaceUnit;

  /// sm spacing value (8pt)
  static const double sm = 0.5 * _spaceUnit;

  /// md spacing value (12pt)
  static const double md = 0.75 * _spaceUnit;

  /// lg spacing value (16pt)
  static const double lg = _spaceUnit;

  /// smxlg spacing value (20pt)
  static const double xlgsm = 1.25 * _spaceUnit;

  /// xlg spacing value (24pt)
  static const double xlg = 1.5 * _spaceUnit;

  /// xxlg spacing value (40pt)
  static const double xxlg = 2.5 * _spaceUnit;

  /// xxxlg pacing value (64pt)
  static const double xxxlg = 4 * _spaceUnit;
}
