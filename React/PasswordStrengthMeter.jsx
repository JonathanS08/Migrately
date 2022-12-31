import React from "react";
import zxcvbn from "zxcvbn";
import PropTypes from "prop-types";

const PasswordStrengthMeter = ({ password }) => {
  const testResult = zxcvbn(password);
  const num = (testResult.score * 100) / 4;

  const createPassLabel = () => {
    switch (testResult.score) {
      case 0:
        return "Very weak";
      case 1:
        return "Weak";
      case 2:
        return "Fear";
      case 3:
        return "Good";
      case 4:
        return "Strong";
      default:
        return "";
    }
  };

  const funcProgressColor = () => {
    switch (testResult.score) {
      case 0:
        return "pwd_very_weak_bgcolor";
      case 1:
        return "pwd_weak_bgcolor";
      case 2:
        return "pwd_fear_bgcolor";
      case 3:
        return "pwd_good_bgcolor";
      case 4:
        return "pwd_strong_bgcolor";
      default:
        return "none";
    }
  };

  const changeProgress = () => {
    switch (num) {
      case 0:
        return "pwd_bar_length_0";
      case 25:
        return "pwd_bar_length_25";
      case 50:
        return "pwd_bar_length_50";
      case 75:
        return "pwd_bar_length_75";
      case 100:
        return "pwd_bar_length_100";
      default:
        return "none";
    }
  };

  const labelColor = () => {
    switch (testResult.score) {
      case 0:
        return "pwd_very_weak_color";
      case 1:
        return "pwd_weak_color";
      case 2:
        return "pwd_fear_color";
      case 3:
        return "pwd_good_color";
      case 4:
        return "pwd_strong_color";
      default:
        return "none";
    }
  };

  return (
    <>
      <div className="progress pwd_progress_bar_height">
        <div
          id="pwd_strength_bar"
          className={`progress-bar pwd_progress_bar_height ${funcProgressColor()} ${changeProgress()}`}
        ></div>
      </div>
      <p className={labelColor()}>{createPassLabel()}</p>
    </>
  );
};

PasswordStrengthMeter.propTypes = {
  password: PropTypes.string.isRequired,
};

export default PasswordStrengthMeter;
