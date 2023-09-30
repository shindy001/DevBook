import 'package:flutter/widgets.dart';
import 'package:flutter_client/dashboard/dashboard_page.dart';
import 'package:go_router/go_router.dart';

abstract class AppRoutes {
  static const home = '/';
  static const dashboard = '/dashboard';
}

GoRouter createRouter() {
  return GoRouter(
    routes: [
      GoRoute(
        path: AppRoutes.home,
        pageBuilder: (context, state) => NoTransitionPage(
          child: Dashboard.routeBuilder(context, state),
        ),
      ),
      GoRoute(
        path: AppRoutes.dashboard,
        pageBuilder: (context, state) => NoTransitionPage(
          child: Dashboard.routeBuilder(context, state),
        ),
      ),
    ],
    observers: [RedirectToHomeObserver()],
  );
}

class RedirectToHomeObserver extends NavigatorObserver {
  @override
  void didPush(Route<dynamic> route, Route<dynamic>? previousRoute) {
    super.didPush(route, previousRoute);

    if (previousRoute == null && route.settings.name != AppRoutes.home) {
      WidgetsBinding.instance.addPostFrameCallback((_) {
        final context = route.navigator!.context;
        GoRouter.of(context).go(AppRoutes.home);
      });
    }
  }
}
